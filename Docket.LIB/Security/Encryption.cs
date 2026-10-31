using System.Security.Cryptography;
using System.Text;

namespace Docket.LIB.Security;

public class Encryption
{
    public string Key = string.Empty;
    public string IV = string.Empty;

    public string Encrypt(string text, string? saltKey = null, string? iv = null)
    {
        using TripleDES tripleDES = TripleDES.Create();
        tripleDES.Key = Encoding.UTF8.GetBytes(saltKey ?? Key);
        tripleDES.IV = Encoding.UTF8.GetBytes(iv ?? IV);
        tripleDES.Mode = CipherMode.CBC;
        tripleDES.Padding = PaddingMode.PKCS7;
        byte[] encryptedTextBlock = tripleDES.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));
        return Encoding.UTF8.GetString(encryptedTextBlock);
    }

    public string Decrypt(string text, string? saltKey = null, string? iv = null)
    {
        using TripleDES tripleDES = TripleDES.Create();
        tripleDES.Key = Encoding.UTF8.GetBytes(saltKey ?? Key);
        tripleDES.IV = Encoding.UTF8.GetBytes(iv ?? IV);
        tripleDES.Mode = CipherMode.CBC;
        tripleDES.Padding = PaddingMode.PKCS7;
        byte[] decryptedTextBlock = tripleDES.CreateDecryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));
        return Encoding.UTF8.GetString(decryptedTextBlock);
    } 
}
