using System.Security.Cryptography;
using ChatSystemBackend.Utils.Interfaces;

namespace ChatSystemBackend.Utils;

public class PasswordUtils : IPasswordUtils
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 100000; // số vòng lặp (tăng để bảo mật hơn)
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;


    public string HashPassword(string password)
    {
        // tạo salt random
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        // tạo key từ password + salt
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            KeySize
        );

        // format lưu: salt.key (base64)
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }


    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            KeySize
        );

        return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
    }
}

