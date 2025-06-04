namespace MiniComp.Core.Extension;

public static class HttpContentExtension
{
    /// <summary>
    /// 转换查询字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetQueryString(this object? obj)
    {
        if (obj == null)
            return string.Empty;
        var qList = new List<string>();
        obj.GetType()
            .GetProperties()
            .ToList()
            .ForEach(d =>
            {
                var val = d.GetValue(obj);
                if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    qList.Add($"{d.Name}={val}");
            });
        return string.Join("&", qList);
    }
}
