using MiniComp.Core.Extension;
using SqlSugar;

namespace MiniComp.Core.DomainAggregateRoot;

public abstract class AggregateRoot : BaseEntity
{
    protected AggregateRoot() { }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(ColumnDataType = "bit", ColumnDescription = "是否启用")]
    public bool Enable { get; set; } = true;

    /// <summary>
    ///是否删除
    /// </summary>
    [SugarColumn(ColumnDataType = "bit", ColumnDescription = "是否删除")]
    public bool IsDel { get; set; }

    /// <summary>
    ///创建人
    /// </summary>
    [SugarColumn(ColumnDataType = "bigint", ColumnDescription = "创建人")]
    public long Creator { get; set; }

    /// <summary>
    ///创建时间
    /// </summary>
    [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "创建时间")]
    public DateTime CreateDate { get; set; } = DateTimeExtension.Now();

    /// <summary>
    ///修改人
    /// </summary>
    [SugarColumn(ColumnDataType = "bigint", ColumnDescription = "修改人", IsNullable = true)]
    public long? Modifier { get; set; }

    /// <summary>
    ///修改时间
    /// </summary>
    [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "修改时间", IsNullable = true)]
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 机构ID
    /// </summary>
    [SugarColumn(ColumnDataType = "bigint", ColumnDescription = "机构ID")]
    public long OrgId { get; set; }

    /// <summary>
    /// 并发更新控制
    /// </summary>
    [SugarColumn(
        ColumnDataType = "bigint",
        IsEnableUpdateVersionValidation = true,
        ColumnDescription = "并发更新控制"
    )]
    public long Version { get; set; }

    protected void Delete()
    {
        IsDel = true;
    }

    protected void ChangeEnable(bool enable)
    {
        Enable = enable;
    }
}
