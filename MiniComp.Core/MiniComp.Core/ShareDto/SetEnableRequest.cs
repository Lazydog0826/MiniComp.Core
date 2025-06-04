namespace MiniComp.Core.ShareDto;

public class SetEnableRequest
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 乐观锁
    /// </summary>
    public long Version { get; set; }
}
