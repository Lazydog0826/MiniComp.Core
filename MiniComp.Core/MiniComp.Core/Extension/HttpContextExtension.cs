using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MiniComp.Core.Extension;

public static class HttpContextExtension
{
    /// <summary>
    /// 获取查询字符串值
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetQueryStringValue(this HttpContext httpContext, string key)
    {
        httpContext.Request.Query.TryGetValue(key, out var sv);
        if (sv.Count == 0)
            httpContext.Request.Query.TryGetValue(key.ToUpper(), out sv);
        if (sv.Count == 0)
            httpContext.Request.Query.TryGetValue(key.ToLower(), out sv);
        return sv.FirstOrDefault() ?? string.Empty;
    }

    /// <summary>
    /// 获取请求头数据
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetRequestHeaderValue(this HttpContext httpContext, string key)
    {
        var header = httpContext.Request.Headers;
        header.TryGetValue(key, out var sv);
        if (sv.Count == 0)
            header.TryGetValue(key.ToUpper(), out sv);
        if (sv.Count == 0)
            header.TryGetValue(key.ToLower(), out sv);
        return sv.FirstOrDefault() ?? string.Empty;
    }

    /// <summary>
    /// 获取响应头数据
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetResponseHeaderValue(this HttpContext httpContext, string key)
    {
        var header = httpContext.Response.Headers;
        header.TryGetValue(key, out var sv);
        if (sv.Count == 0)
            header.TryGetValue(key.ToUpper(), out sv);
        if (sv.Count == 0)
            header.TryGetValue(key.ToLower(), out sv);
        return sv.FirstOrDefault() ?? string.Empty;
    }

    /// <summary>
    /// 获取服务端IP
    /// </summary>
    /// <returns></returns>
    public static string GetServerIp(this HttpContext httpContext)
    {
        var hostIp = new StringBuilder();
        if (httpContext.Request.Host.HasValue)
            hostIp.Append(httpContext.Request.Host.Host);
        if (httpContext.Request.Host.Port.HasValue)
            hostIp.Append($":{httpContext.Request.Host.Port.ToString()}");
        if (!string.IsNullOrEmpty(hostIp.ToString()))
            return hostIp.ToString();
        var hostName = Dns.GetHostName();
        var addresses = Dns.GetHostAddresses(hostName, AddressFamily.InterNetwork);
        hostIp.Append(string.Join(",", addresses.ToList()));
        return hostIp.ToString();
    }

    /// <summary>
    /// 获取客户端IP
    /// </summary>
    /// <returns></returns>
    public static string GetClientIp(this HttpContext httpContext)
    {
        var ip = httpContext.GetRequestHeaderValue("X-Real-IP");
        if (string.IsNullOrEmpty(ip))
            ip = httpContext.GetRequestHeaderValue("X-Forwarded-For");
        if (string.IsNullOrEmpty(ip))
            ip = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;
        return ip;
    }

    /// <summary>
    /// 获取请求路由
    /// </summary>
    /// <returns></returns>
    public static string GetRequestRouterPath(this HttpContext httpContext)
    {
        return httpContext.Request.Path.ToString();
    }

    /// <summary>
    /// 获取元数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetMetaData<T>(this HttpContext httpContext)
        where T : class
    {
        return httpContext.GetEndpoint()?.Metadata.GetMetadata<T>();
    }

    /// <summary>
    /// 请求是否是gRPC请求
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static bool IsGrpc(this HttpContext httpContext) =>
        httpContext.GetRequestHeaderValue("Content-Type").EqualsIgnoreCase("application/grpc");
}
