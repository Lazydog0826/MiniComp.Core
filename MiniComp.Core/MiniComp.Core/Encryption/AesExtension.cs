using System.Security.Cryptography;
using System.Text;

namespace MiniComp.Core.Encryption;

public static class AesExtension
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <param name="keySize"></param>
    /// <param name="blockSize"></param>
    /// <param name="encoding"></param>
    /// <param name="paddingMode"></param>
    /// <param name="cipherMode"></param>
    /// <returns></returns>
    public static string AesEncryption(
        this string str,
        string key,
        string iv,
        int keySize = 128,
        int blockSize = 128,
        Encoding? encoding = null,
        PaddingMode paddingMode = PaddingMode.PKCS7,
        CipherMode cipherMode = CipherMode.CBC
    )
    {
        encoding ??= Encoding.UTF8;
        var textBytes = encoding.GetBytes(str);
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.BlockSize = blockSize;
        aes.Padding = paddingMode;
        aes.Mode = cipherMode;
        aes.Key = encoding.GetBytes(key);
        aes.IV = encoding.GetBytes(iv);
        var cryptoTransform = aes.CreateEncryptor();
        var resultBytes = cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
        return Convert.ToBase64String(resultBytes);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <param name="keySize"></param>
    /// <param name="blockSize"></param>
    /// <param name="encoding"></param>
    /// <param name="paddingMode"></param>
    /// <param name="cipherMode"></param>
    /// <returns></returns>
    public static string AesDecryption(
        this string str,
        string key,
        string iv,
        int keySize = 128,
        int blockSize = 128,
        Encoding? encoding = null,
        PaddingMode paddingMode = PaddingMode.PKCS7,
        CipherMode cipherMode = CipherMode.CBC
    )
    {
        encoding ??= Encoding.UTF8;
        var textBytes = Convert.FromBase64String(str);
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.BlockSize = blockSize;
        aes.Padding = paddingMode;
        aes.Mode = cipherMode;
        aes.Key = encoding.GetBytes(key);
        aes.IV = encoding.GetBytes(iv);
        var decryptor = aes.CreateDecryptor();
        var resultBytes = decryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
        return encoding.GetString(resultBytes);
    }
}
