using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace MiniComp.Core.Extension;

public static class ObjectExtension
{
    #region 类型判断

    /// <summary>
    /// 是否是集合
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsList(this Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

    /// <summary>
    /// 是否为基础类型（所有值类型+string类型）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsValueTypeOrString(this Type type) =>
        type.IsValueType || type == typeof(string);

    /// <summary>
    /// 是否是可空值类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullableType(this Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>
    /// 获取可空值类型的根类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetNullableRootType(this Type type) =>
        type.IsNullableType() ? type.GetGenericArguments().First() : type;

    /// <summary>
    /// 获取类型所有继承的类
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<Type> GetAllExtendType(this Type type)
    {
        var res = new List<Type>();
        var baseType = type.BaseType;
        while (baseType != null)
        {
            res.Add(baseType);
            baseType = baseType.BaseType;
        }
        return res;
    }

    #endregion 类型判断

    #region 枚举扩展

    /// <summary>
    /// 获取枚举的描述属性
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns></returns>
    public static string GetEnumDescription(this Enum enumValue)
    {
        var at = enumValue.GetEnumAttribute<DescriptionAttribute>();
        return at == null ? enumValue.ToString() : at.Description;
    }

    /// <summary>
    /// 获取枚举属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static T? GetEnumAttribute<T>(this Enum enumValue)
        where T : Attribute
    {
        var value = enumValue.ToString();
        var field = enumValue.GetType().GetField(value)!;
        var at = field.GetCustomAttribute<T>();
        return at;
    }

    #endregion 枚举扩展

    #region 类型转换

    /// <summary>
    /// 转换类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="data">原数据</param>
    /// <param name="defaultVal">默认值</param>
    /// <param name="isDefaultNullThrowException">默认值为空时是否抛出异常</param>
    /// <param name="isExceptionReturnDefault">转换失败时是否返回默认值</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T? ChangeType<T>(
        this object? data,
        object? defaultVal = null,
        bool isDefaultNullThrowException = false,
        bool isExceptionReturnDefault = true
    )
    {
        var res = data.ChangeType(
            typeof(T),
            defaultVal,
            isDefaultNullThrowException,
            isExceptionReturnDefault
        );
        return res == null ? default : (T)res;
    }

    /// <summary>
    /// 转换类型
    /// </summary>
    /// <param name="data">原数据</param>
    /// <param name="type"></param>
    /// <param name="defaultVal">默认值</param>
    /// <param name="isDefaultNullThrowException">默认值为空时是否抛出异常</param>
    /// <param name="isExceptionReturnDefault">转换失败时是否返回默认值</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static object? ChangeType(
        this object? data,
        Type type,
        object? defaultVal = null,
        bool isDefaultNullThrowException = false,
        bool isExceptionReturnDefault = true
    )
    {
        object? GetDefault()
        {
            if (defaultVal != null)
            {
                try
                {
                    var defaultValByType = defaultVal.ChangeType(type, null, true, true);
                    return defaultValByType;
                }
                catch
                {
                    throw new Exception("尝试变更默认值类型失败");
                }
            }
            if (isDefaultNullThrowException)
                throw new Exception("默认值为空");
            return null;
        }

        // 如果源对象是null则返回默认值
        if (data == null)
            return GetDefault();
        // 如果源对象类型与目标类型相同，则直接返回
        if (data.GetType() == type || type.IsInstanceOfType(data))
            return data;
        try
        {
            // 枚举需要特殊处理
            return type.IsEnum
                // 上面已经非空判断，可以data已经不为null
                ? Enum.Parse(type, data.ToString()!)
                : Convert.ChangeType(
                    data,
                    type.IsNullableType() ? type.GetNullableRootType() : type
                );
        }
        catch
        {
            if (isExceptionReturnDefault)
                return GetDefault();
            throw new Exception("目标值变更类型失败");
        }
    }

    #endregion 类型转换

    #region 集合扩展

    /// <summary>
    /// 集合未空并有值判断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool NoNullAny<T>(this List<T>? list) => list != null && list.Count != 0;

    #endregion 集合扩展

    #region 字符串

    /// <summary>
    /// 比较字符串（忽略大小写）
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="isCompatibleNull"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(
        this string? str1,
        string? str2,
        bool isCompatibleNull = true
    )
    {
        if (isCompatibleNull)
        {
            return (str1 ?? string.Empty).Equals(
                (str2 ?? string.Empty),
                StringComparison.OrdinalIgnoreCase
            );
        }
        if (str1 == null && str2 == null)
        {
            return true;
        }
        if (str1 == null || str2 == null)
        {
            return false;
        }
        return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断字符串是否为空，如果是返回给定值
    /// </summary>
    /// <param name="val"></param>
    /// <param name="val2"></param>
    /// <returns></returns>
    public static string IsNullOrGivenValue(this string val, string? val2 = null)
    {
        val2 ??= string.Empty;
        return string.IsNullOrWhiteSpace(val) ? val2 : val;
    }

    /// <summary>
    /// 字符串截取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static List<T> StrSplit<T>(this string? str, string separator = ",")
    {
        return str == null ? [] : str.Split(separator).Select(x => x.ChangeType<T>()!).ToList();
    }

    /// <summary>
    /// 集合拼接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinToStr<T>(this IEnumerable<T> list, string separator = ",")
    {
        return string.Join(separator, list);
    }

    #endregion 字符串

    #region 获取项目中所有类型

    /// <summary>
    /// 获取项目所有程序集
    /// </summary>
    /// <returns></returns>
    public static List<Assembly> GetProjectAllAssembly()
    {
        var deps = DependencyContext.Default;
        if (deps == null)
            return [];
        var libs = deps.RuntimeLibraries.Where(lib => lib.Type == "project").ToList();
        return libs.Select(x => Assembly.Load(x.Name)).ToList();
    }

    /// <summary>
    /// 获取项目中所有类型
    /// </summary>
    /// <returns></returns>
    public static List<Type> GetProjectAllType()
    {
        var types = GetProjectAllAssembly().SelectMany(d => d.GetTypes()).ToList();
        return types;
    }

    #endregion 获取项目中所有类型

    #region 属性

    /// <summary>
    /// 类型是否包含属性
    /// </summary>
    /// <param name="type"></param>
    /// <param name="prop"></param>
    /// <param name="stringComparison"></param>
    /// <returns></returns>
    public static bool IncludeProp(
        this Type type,
        string prop,
        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase
    ) => type.GetProperties().Any(x => x.Name.Equals(prop, stringComparison));

    #endregion 属性
}
