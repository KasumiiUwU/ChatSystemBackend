using System.Net;
using System.Reflection;
using ChatSystemBackend.Exceptions;

namespace ChatSystemBackend.Utils;

public static class EnumHelper
{
    public static TEnum ToEnum<TEnum>(string value, bool ignoreCase = true) where TEnum : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase, out TEnum result))
            return result;

        throw new CustomExceptions.InvalidDataException( nameof(HttpStatusCode.BadRequest), "Invalid value");
    }

    public static string ToStringValue<TEnum>(TEnum enumValue) where TEnum : struct, Enum
    {
        return enumValue.ToString();
    }
}