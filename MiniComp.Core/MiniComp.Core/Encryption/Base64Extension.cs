using System.Text;

namespace MiniComp.Core.Encryption;

public static class Base64Extension
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="source"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Base64Encryption(this string source, Encoding encoding)
    {
        var bytes = encoding.GetBytes(source);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Base64Decryption(this string base64, Encoding encoding)
    {
        var bytes = Convert.FromBase64String(base64);
        return encoding.GetString(bytes);
    }
}
