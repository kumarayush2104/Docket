using Crypt = BCrypt.Net.BCrypt;

namespace Docket.LIB.Security;

static public class Hash
{
    public static string Encrypt(this string text) => Crypt.HashPassword(text);
    public static bool Verify(string text, string hash) => Crypt.Verify(text, hash);
}