using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniComp.Core.Extension;

namespace MiniComp.Core.App;

public static class HostApp
{
    public static IConfiguration Configuration { get; private set; } = null!;
    public static IServiceProvider RootServiceProvider { get; private set; } = null!;
    public static List<Assembly> AppAssemblyList { get; private set; } = null!;
    public static List<Type> AppDomainTypes { get; private set; } = null!;
    public static IHostEnvironment HostEnvironment { get; private set; } = null!;
    public static string AppRootPath { get; private set; } = null!;

    public static async Task StartWebAppAsync(
        string[] args,
        Func<WebApplicationBuilder, Task> builderFunc,
        Func<WebApplication, Task> appFunc
    )
    {
        var builder = WebApplication.CreateBuilder(args);
        AppAssemblyList = ObjectExtension.GetProjectAllAssembly();
        AppDomainTypes = ObjectExtension.GetProjectAllType();
        Configuration = builder.Configuration;
        HostEnvironment = builder.Environment;
        AppRootPath = AppContext.BaseDirectory;
        builder.Services.AddLogging();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();
        await builderFunc(builder);
        var app = builder.Build();
        RootServiceProvider = app.Services;
        app.UseRouting();
        app.MapHealthChecks("/Health");
        app.MapHealthChecks("/Healthz");
        app.MapHealthChecks("/");
        await appFunc(app);
        await app.RunAsync();
    }

    public static async Task StartConsoleAppAsync(
        string[] args,
        Func<IHostBuilder, Task> builderFunc,
        Func<IHost, Task> appFunc
    )
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(
                (hostContext, services) =>
                {
                    Configuration = hostContext.Configuration;
                    HostEnvironment = hostContext.HostingEnvironment;
                    services.AddLogging();
                }
            );
        AppAssemblyList = ObjectExtension.GetProjectAllAssembly();
        AppDomainTypes = ObjectExtension.GetProjectAllType();
        AppRootPath = AppContext.BaseDirectory;
        await builderFunc(builder);
        var app = builder.Build();
        RootServiceProvider = app.Services;
        await appFunc(app);
        await app.RunAsync();
    }
}
