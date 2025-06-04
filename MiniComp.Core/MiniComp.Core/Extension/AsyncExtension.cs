namespace MiniComp.Core.Extension;

public static class AsyncExtension
{
    /// <summary>
    /// 异步ForEach使用结构语法foreach
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static async Task ForEachAsync<T>(this IEnumerable<T> values, Func<T, Task> func)
    {
        foreach (var value in values)
        {
            await func.Invoke(value);
        }
    }

    /// <summary>
    /// 异步Select使用ForEachAsync方法遍历
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="values"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static async Task<List<TResult>> SelectAsync<T, TResult>(
        this IEnumerable<T> values,
        Func<T, Task<TResult>> func
    )
    {
        var res = new List<TResult>();
        await values.ForEachAsync(async d =>
        {
            var data = await func.Invoke(d);
            res.Add(data);
        });
        return res;
    }
}
