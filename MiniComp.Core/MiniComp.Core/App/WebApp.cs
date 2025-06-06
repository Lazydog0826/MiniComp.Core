using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniComp.Core.Extension;

namespace MiniComp.Core.App;

public static class WebApp
{
    /// <summary>
    /// http上下文
    /// </summary>
    public static HttpContext? HttpContext =>
        HostApp.RootServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext;

    /// <summary>
    /// 当前异步控制流的IServiceProvider
    /// </summary>
    private static readonly AsyncLocal<IServiceProvider> AsyncLocalServiceProvider = new();

    /// <summary>
    /// 创建新的服务域
    /// </summary>
    private static void CreateNewScopeServiceProvider()
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
            if (HttpContext != null)
            {
                AsyncLocalServiceProvider.Value = HttpContext.RequestServices;
                return AsyncLocalServiceProvider.Value;
            }
            CreateNewScopeServiceProvider();
            return AsyncLocalServiceProvider.Value!;
        }
    }

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
