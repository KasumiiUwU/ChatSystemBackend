namespace ChatSystemBackend.Utils.Interfaces;

public interface IPasswordUtils
{
    
    /// <summary>
    /// Tạo hash kèm salt cho password
    /// </summary>
    string HashPassword(string password);
    
    /// <summary>
    /// Verify password input với hash đã lưu
    /// </summary>
    bool VerifyPassword(string password, string hashedPassword);

}