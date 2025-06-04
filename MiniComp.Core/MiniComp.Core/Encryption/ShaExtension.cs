using System.Security.Cryptography;
using System.Text;

namespace MiniComp.Core.Encryption;

public static class ShaExtension
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isLower"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Sha1Encryption(
        this string str,
        bool isLower = true,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        var hash = SHA1.HashData(encoding.GetBytes(str));
        var shaStr = Convert.ToHexString(hash);
        return isLower ? shaStr.ToLower() : shaStr.ToUpper();
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isLower"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Sha256Encryption(
        this string str,
        bool isLower = true,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        var hash = SHA256.HashData(encoding.GetBytes(str));
        var shaStr = Convert.ToHexString(hash);
        return isLower ? shaStr.ToLower() : shaStr.ToUpper();
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isLower"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Sha512Encryption(
        this string str,
        bool isLower = true,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        var hash = SHA512.HashData(encoding.GetBytes(str));
        var shaStr = Convert.ToHexString(hash);
        return isLower ? shaStr.ToLower() : shaStr.ToUpper();
    }
}
