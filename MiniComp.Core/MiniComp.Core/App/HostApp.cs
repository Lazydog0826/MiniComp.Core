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

    private static readonly string EnvironmentName =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    public static async Task StartWebAppAsync(
        string[] args,
        Func<WebApplicationBuilder, Task> builderFunc,
        Func<WebApplication, Task> appFunc
    )
    {
        var baseDirectory = AppContext.BaseDirectory;
        var builder = WebApplication.CreateEmptyBuilder(
            new WebApplicationOptions
            {
                Args = args,
                EnvironmentName = EnvironmentName,
                ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name,
                ContentRootPath = baseDirectory,
                WebRootPath = Path.Combine(baseDirectory, "wwwroot"),
            }
        );
        AppAssemblyList = ObjectExtension.GetProjectAllAssembly();
        AppDomainTypes = ObjectExtension.GetProjectAllType();
        Configuration = builder.Configuration;
        HostEnvironment = builder.Environment;
        AppRootPath = baseDirectory;
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
        Func<HostApplicationBuilder, Task> builderFunc,
        Func<IHost, Task> appFunc
    )
    {
        var builder = Host.CreateEmptyApplicationBuilder(
            new HostApplicationBuilderSettings
            {
                ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name,
                Args = args,
                Configuration = null,
                ContentRootPath = AppContext.BaseDirectory,
                DisableDefaults = false,
                EnvironmentName = EnvironmentName,
            }
        );
        AppAssemblyList = ObjectExtension.GetProjectAllAssembly();
        AppDomainTypes = ObjectExtension.GetProjectAllType();
        Configuration = builder.Configuration;
        HostEnvironment = builder.Environment;
        AppRootPath = AppContext.BaseDirectory;
        builder.Services.AddLogging();
        await builderFunc(builder);
        var app = builder.Build();
        await appFunc(app);
        await app.RunAsync();
    }
}
