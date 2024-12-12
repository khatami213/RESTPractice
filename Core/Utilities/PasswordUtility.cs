using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities;

public static class PasswordUtility
{
    public static string ComputeHash(string password, string salt)
    {
        var md5 = MD5.Create();
        var sha256 = SHA256.Create();

        var passwordSalt = $"{password}{salt}";

        var byteValue = Encoding.UTF8.GetBytes(passwordSalt);

        var result = md5.ComputeHash(sha256.ComputeHash(byteValue));

        return Convert.ToBase64String(result);
    }

    public static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
}
