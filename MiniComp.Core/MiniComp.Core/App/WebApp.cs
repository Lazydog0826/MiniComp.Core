using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniComp.Core.Extension;

namespace MiniComp.Core.App;

public static class WebApp
{
    #region HttpContext

    /// <summary>
    /// 当前异步控制流的HttpContext
    /// </summary>
    private static readonly AsyncLocal<IHttpContextAccessor> AsyncLocalHttpContextAccessor = new();

    /// <summary>
    /// 设置HttpContext
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public static void SetHttpContext(IHttpContextAccessor httpContextAccessor) =>
        AsyncLocalHttpContextAccessor.Value = httpContextAccessor;

    /// <summary>
    /// http上下文
    /// </summary>
    public static HttpContext? HttpContext => AsyncLocalHttpContextAccessor.Value?.HttpContext;

    #endregion HttpContext

    #region IServiceProvider

    /// <summary>
    /// 当前异步控制流的IServiceProvider
    /// </summary>
    private static readonly AsyncLocal<IServiceProvider> AsyncLocalServiceProvider = new();

    /// <summary>
    /// 创建新的服务域
    /// </summary>
    public static void CreateNewScopeServiceProvider()
    {
        AsyncLocalServiceProvider.Value = HostApp
            .RootServiceProvider.CreateAsyncScope()
            .ServiceProvider;
    }

    /// <summary>
    /// 服务提供器
    /// </summary>
    public static IServiceProvider ServiceProvider
    {
        get
        {
            if (AsyncLocalServiceProvider.Value != null)
            {
                return AsyncLocalServiceProvider.Value;
            }
            else if (HttpContext != null)
            {
                AsyncLocalServiceProvider.Value = HttpContext.RequestServices;
                return AsyncLocalServiceProvider.Value;
            }
            else
            {
                CreateNewScopeServiceProvider();
                return AsyncLocalServiceProvider.Value!;
            }
        }
    }

    #endregion IServiceProvider

    /// <summary>
    /// 异步上下文数据
    /// </summary>
    private static readonly AsyncLocal<Dictionary<string, string>> AsyncLocalDic = new();

    /// <summary>
    /// 新增值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void AddValue(string key, string value)
    {
        AsyncLocalDic.Value ??= [];
        AsyncLocalDic.Value.Add(key, value);
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetValue<T>(string key)
    {
        AsyncLocalDic.Value ??= [];
        return AsyncLocalDic.Value.TryGetValue(key, out var val) ? val.ChangeType<T>() : default;
    }
}
