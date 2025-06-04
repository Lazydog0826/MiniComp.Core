using Microsoft.Extensions.Configuration;

namespace MiniComp.Core.Extension;

public static class ConfigurationExtension
{
    public static T Configuration<T>(
        this IConfiguration configuration,
        bool isDefault = false,
        string? key = null
    )
        where T : new()
    {
        key ??= typeof(T).Name;
        var config = configuration.GetSection(key).Get<T>();
        return config != null ? config
            : isDefault == false ? throw new Exception($"配置缺失：{key}")
            : new T();
    }
}
