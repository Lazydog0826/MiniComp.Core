using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniComp.Core.App;
using MiniComp.Core.Extension;

namespace MiniComp.Core;

public static class Setup
{
    /// <summary>
    /// 根据环境变量加载Json配置
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static ConfigurationManager AddJsonFileByEnvironment(
        this ConfigurationManager manager,
        string path
    )
    {
        var appSettingsPath = Path.Join(HostApp.AppRootPath, path);
        if (!Directory.Exists(appSettingsPath))
            return manager;
        // 获取所有配置文件
        var paths = IoExtension.GetFileSystemInfos(
            appSettingsPath,
            true,
            x => x.Extension.EqualsIgnoreCase(".json")
        );
        var jsonFileReg = JsonFileRegex.DefaultJsonFileRegex();
        var envJsonFileReg = JsonFileRegex.EnvJsonFileRegex(
            HostApp.HostEnvironment.EnvironmentName
        );
        // 用正则筛选出配置并添加进程序
        var addJsonFileList = paths.Where(x => !jsonFileReg.IsMatch(x.Name)).ToList();
        addJsonFileList.AddRange([.. paths.Where(x => envJsonFileReg.IsMatch(x.Name))]);
        addJsonFileList.ForEach(d => manager.AddJsonFile(d.FullName));
        return manager;
    }

    /// <summary>
    /// 添加跨域（开发环境自动放开所有限制）
    /// </summary>
    /// <param name="services"></param>
    /// <param name="origins"></param>
    /// <param name="headers"></param>
    /// <param name="methods"></param>
    /// <param name="corsName"></param>
    /// <returns></returns>
    public static IServiceCollection AddCors(
        this IServiceCollection services,
        string origins,
        string headers,
        string methods,
        string? corsName = null
    )
    {
        services.AddCors(opt =>
        {
            if (string.IsNullOrWhiteSpace(corsName))
            {
                opt.AddDefaultPolicy(SetCorsPolicyBuilder);
            }
            else
            {
                opt.AddPolicy(corsName, SetCorsPolicyBuilder);
            }
        });
        return services;

        void SetCorsPolicyBuilder(CorsPolicyBuilder policy)
        {
            if (HostApp.HostEnvironment.IsDevelopment())
            {
                policy
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
            else
            {
                policy
                    .WithOrigins(origins.StrSplit<string>().ToArray())
                    .WithHeaders(headers.StrSplit<string>().ToArray())
                    .WithMethods(methods.StrSplit<string>().ToArray())
                    .AllowCredentials();
            }
        }
    }
}
