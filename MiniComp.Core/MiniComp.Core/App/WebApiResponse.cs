using System.Net;

namespace MiniComp.Core.App;

/// <summary>
/// WebApiResponse
/// </summary>
public class WebApiResponse
{
    private WebApiResponse(
        HttpStatusCode httpStatusCode,
        string message,
        object? data,
        IDictionary<string, object>? errors
    )
    {
        Code = httpStatusCode;
        Message = message;
        Data = data;
        Errors = errors;
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public HttpStatusCode Code { get; private set; }

    /// <summary>
    /// 信息说明（默认成功，失败会在异常拦截器处理异常字段）
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// 响应数据
    /// </summary>
    public object? Data { get; private set; }

    /// <summary>
    /// 日志ID
    /// </summary>
    public long ApiLogId => WebApp.GetValue<long>(nameof(ApiLogId));

    /// <summary>
    /// 模型验证错误信息
    /// </summary>
    public IDictionary<string, object>? Errors { get; private set; }

    /// <summary>
    /// 错误响应
    /// </summary>
    /// <param name="message"></param>
    /// <param name="httpStatusCode"></param>
    /// <param name="errorModels"></param>
    /// <returns></returns>
    public static WebApiResponse Error(
        string message,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
        IDictionary<string, object>? errorModels = null
    )
    {
        return new WebApiResponse(httpStatusCode, message, null, errorModels);
    }

    /// <summary>
    /// 查询响应
    /// </summary>
    /// <param name="httpStatusCode"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static WebApiResponse Query(
        object? data = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        string message = "成功"
    )
    {
        return new WebApiResponse(httpStatusCode, message, data, null);
    }

    /// <summary>
    /// 操作响应
    /// </summary>
    /// <param name="httpStatusCode"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static WebApiResponse Operate(
        HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        string message = "成功",
        object? data = null
    )
    {
        return new WebApiResponse(httpStatusCode, message, data, null);
    }
}
