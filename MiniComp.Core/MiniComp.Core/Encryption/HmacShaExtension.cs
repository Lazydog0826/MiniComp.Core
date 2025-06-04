using System.Text;

namespace MiniComp.Core.Encryption;

public static class HmacShaExtension
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string HmacSha256Encryption(
        this string str,
        string key,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        using var mac = new System.Security.Cryptography.HMACSHA256(encoding.GetBytes(key));
        var hash = mac.ComputeHash(encoding.GetBytes(str));
        var signRet = Convert.ToBase64String(hash);
        return signRet;
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string HmacSha1Encryption(this string str, string key, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        using var mac = new System.Security.Cryptography.HMACSHA1(encoding.GetBytes(key));
        var hash = mac.ComputeHash(encoding.GetBytes(str));
        var signRet = Convert.ToBase64String(hash);
        return signRet;
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string HmacSha512Encryption(
        this string str,
        string key,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        using var mac = new System.Security.Cryptography.HMACSHA512(encoding.GetBytes(key));
        var hash = mac.ComputeHash(encoding.GetBytes(str));
        var signRet = Convert.ToBase64String(hash);
        return signRet;
    }
}
