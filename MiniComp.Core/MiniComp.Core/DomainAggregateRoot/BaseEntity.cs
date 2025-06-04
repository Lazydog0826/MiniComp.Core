using SqlSugar;

namespace MiniComp.Core.DomainAggregateRoot;

public abstract class BaseEntity
{
    protected BaseEntity() { }

    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnDataType = "bigint", ColumnDescription = "主键ID", IsPrimaryKey = true)]
    public long Id { get; set; }
}
