using Microsoft.Extensions.Configuration;

namespace MiniComp.Core.Extension;

public static class ConfigurationExtension
{
    public static T Configuration<T>(this IConfiguration configuration, string? key = null)
    {
        key ??= typeof(T).Name;
        var config = configuration.GetSection(key).Get<T>();
        return config ?? throw new Exception($"配置项缺失：{key}");
    }
}
