using System.Text;

namespace MiniComp.Core.Encryption;

public static class Md5Extension
{
    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="is16"></param>
    /// <param name="isLower"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Md5Encryption(
        this string str,
        bool is16 = false,
        bool isLower = true,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        var resultBytes = System.Security.Cryptography.MD5.HashData(encoding.GetBytes(str));
        var resStr = Convert.ToHexString(resultBytes);
        resStr = is16 ? resStr[8..24] : resStr;
        resStr = isLower ? resStr.ToLower() : resStr.ToUpper();
        return resStr;
    }
}
