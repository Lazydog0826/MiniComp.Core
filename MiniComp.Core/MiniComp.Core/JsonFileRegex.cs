using System.Text.RegularExpressions;

namespace MiniComp.Core;

public static partial class JsonFileRegex
{
    [GeneratedRegex(@"\w{1,}\.\w{1,}\.json", RegexOptions.IgnoreCase, "zh-CN")]
    public static partial Regex DefaultJsonFileRegex();

    public static Regex EnvJsonFileRegex(string env)
    {
        return new Regex(@"\w{1,}\." + env + @"\.json", RegexOptions.IgnoreCase);
    }
}
