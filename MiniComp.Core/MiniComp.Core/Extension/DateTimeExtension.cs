using MiniComp.Core.App;
using NodaTime;

namespace MiniComp.Core.Extension;

public static class DateTimeExtension
{
    /// <summary>
    /// 获取时间戳（毫秒）
    /// </summary>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    public static long GetTimestamp(string? timeZone = null)
    {
        return new DateTimeOffset(Now(timeZone)).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 获取时间戳（秒）
    /// </summary>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    public static long GetShortTimestamp(string? timeZone = null)
    {
        return new DateTimeOffset(Now(timeZone)).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 时间操作辅助，默认取上海时区时间
    /// </summary>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    public static DateTime Now(string? timeZone = null)
    {
        timeZone ??= "Asia/Shanghai";
        var now = SystemClock.Instance.GetCurrentInstant();
        return now.InZone(DateTimeZoneProviders.Tzdb[timeZone]).ToDateTimeUnspecified();
    }

    /// <summary>
    /// 转换时间条件的开始时间和结束时间
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static (DateTime?, DateTime?) ConvertDateWhere(DateTime? begin, DateTime? end)
    {
        var beginRes = begin?.Date;
        var endRes = end?.Date.AddDays(1).AddSeconds(-1);
        return (beginRes, endRes);
    }
}
