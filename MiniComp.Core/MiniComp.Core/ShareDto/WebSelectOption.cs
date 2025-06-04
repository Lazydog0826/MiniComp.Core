namespace MiniComp.Core.ShareDto;

public class WebSelectOption<TLabel, TValue>
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 父级
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// Label
    /// </summary>
    public TLabel Label { get; set; } = default!;

    /// <summary>
    /// Value
    /// </summary>
    public TValue Value { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 额外配置
    /// </summary>
    public object Config { get; set; } = new { };

    /// <summary>
    /// 组
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// 树形结构需要下级
    /// </summary>
    public List<WebSelectOption<TLabel, TValue>> Children { get; set; } = [];
}
