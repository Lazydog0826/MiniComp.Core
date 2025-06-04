using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MiniComp.Core.App;

public static class HostApp
{
    private static IConfiguration _configuration = null!;
    private static IServiceProvider _rootServiceProvider = null!;
    private static List<Assembly> _appAssemblyList = null!;
    private static List<Type> _appDomainTypes = null!;
    private static IHostEnvironment _hostEnvironment = null!;
    private static string _appRootPath = null!;

    public static IConfiguration Configuration
    {
        get => _configuration;
        set
        {
            if (_configuration != null)
                throw new Exception("重复赋值异常");
            _configuration = value;
        }
    }

    public static IServiceProvider RootServiceProvider
    {
        get => _rootServiceProvider;
        set
        {
            if (_rootServiceProvider != null)
                throw new Exception("重复赋值异常");
            _rootServiceProvider = value;
        }
    }

    public static List<Assembly> AppAssemblyList
    {
        get => _appAssemblyList;
        set
        {
            if (_appAssemblyList != null)
                throw new Exception("重复赋值异常");
            _appAssemblyList = value;
        }
    }

    public static List<Type> AppDomainTypes
    {
        get => _appDomainTypes;
        set
        {
            if (_appDomainTypes != null)
                throw new Exception("重复赋值异常");
            _appDomainTypes = value;
        }
    }

    public static IHostEnvironment HostEnvironment
    {
        get => _hostEnvironment;
        set
        {
            if (_hostEnvironment != null)
                throw new Exception("重复赋值异常");
            _hostEnvironment = value;
        }
    }

    public static string AppRootPath
    {
        get => _appRootPath;
        set
        {
            if (_appRootPath != null)
                throw new Exception("重复赋值异常");
            _appRootPath = value;
        }
    }
}
