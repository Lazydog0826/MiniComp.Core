using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MiniComp.Core.App.JsonConverter;

public static class NewtonsoftJsonConfiguration
{
    public static Action<JsonSerializerSettings> Configure =>
        (settings) =>
        {
            // 忽略循环引用
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // 数据格式首字母小写（小驼峰）
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // 设置日期格式
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            // long类型转string
            settings.Converters.Add(new LongToStringConverter());
            // 枚举转字符串
            settings.Converters.Add(new StringEnumConverter());

            // 包含为null的值
            settings.NullValueHandling = NullValueHandling.Ignore;

            // 忽略额外的值
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            // 设置默认值
            settings.DefaultValueHandling = DefaultValueHandling.Include;
        };
}
