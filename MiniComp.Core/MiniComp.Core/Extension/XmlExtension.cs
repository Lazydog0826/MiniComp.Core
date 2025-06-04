using System.Text;
using System.Xml.Serialization;

namespace MiniComp.Core.Extension;

/// <summary>
/// 控制XML序列化特性请参考：https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/attributes-that-control-xml-serialization
/// </summary>
public static class XmlExtension
{
    /// <summary>
    /// 转XML
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string SerializationXml(this object obj)
    {
        using var ms = new MemoryStream();
        var serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(ms, obj);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    /// <summary>
    /// 转对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static T? DeserializationXml<T>(this string xml)
    {
        using var sr = new StringReader(xml);
        var serializer = new XmlSerializer(typeof(T));
        var obj = serializer.Deserialize(sr);
        return obj!.ChangeType<T>();
    }
}
