using System.Text;

namespace MiniComp.Core.Extension;

public static class IoExtension
{
    /// <summary>
    /// 获取路径下所有文件夹或文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isRecursive"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static List<FileSystemInfo> GetFileSystemInfos(
        string path,
        bool isRecursive = true,
        Func<FileSystemInfo, bool>? func = null
    )
    {
        var di = new DirectoryInfo(path);
        var fileSystemInfos = di.GetFileSystemInfos();
        var res = fileSystemInfos.ToList();
        if (isRecursive)
        {
            fileSystemInfos
                .ToList()
                .ForEach(d =>
                {
                    if (d.Attributes == FileAttributes.Directory)
                        res.AddRange(GetFileSystemInfos(d.FullName, isRecursive, func));
                });
        }
        if (func != null)
            res = res.Where(func.Invoke).ToList();
        return res;
    }

    /// <summary>
    /// 读取文件内容
    /// </summary>
    /// <param name="encoding"></param>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static async Task<string> ReadFileAsync(Encoding encoding, params string[] paths)
    {
        return await File.ReadAllTextAsync(Path.Join(paths), encoding);
    }
}
