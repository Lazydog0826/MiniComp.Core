using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MiniComp.Core.Encryption;

public class JwtExtension
{
    /// <summary>
    /// 生成TOKEN
    ///
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="encryptionKeyEncoding"></param>
    /// <returns></returns>
    public static string GetToken(
        Dictionary<string, string> dic,
        string encryptionKey,
        Encoding? encryptionKeyEncoding = null
    )
    {
        encryptionKeyEncoding ??= Encoding.UTF8;
        var claims = dic.Keys.Select(key => new Claim(key, dic[key].ToString())).ToList();
        var jwtKey = new SymmetricSecurityKey(encryptionKeyEncoding.GetBytes(encryptionKey));
        var jwtHeader = new JwtHeader(
            new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256)
        );

        // JwtRegisteredClaimNames.Aud
        // JwtRegisteredClaimNames.Iss
        // JwtRegisteredClaimNames.Sub
        // JwtRegisteredClaimNames.Iat
        // JwtRegisteredClaimNames.Exp
        // JwtRegisteredClaimNames.Nbf
        // JwtRegisteredClaimNames.Jti

        var jwtPayload = new JwtPayload(claims);
        var jwtToken = new JwtSecurityToken(jwtHeader, jwtPayload);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return token;
    }

    /// <summary>
    /// 解密Token成字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> GetTokenValue(string token)
    {
        var st = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var claims = st.Claims.ToList();
        var res = new Dictionary<string, string>();
        claims.ForEach(d =>
        {
            res.Add(d.Type, d.Value);
        });
        return res;
    }

    /// <summary>
    /// 验证Token
    /// </summary>
    /// <param name="token"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static bool ValidationToken(string token, TokenValidationParameters parameters)
    {
        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
