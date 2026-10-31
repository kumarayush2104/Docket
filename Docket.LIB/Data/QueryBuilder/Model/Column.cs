namespace Docket.LIB.Data.QueryBuilder.Model;

public class Column
{
    required public string ColumnName { get; set; }
    public string? TableName { get; set; }
    public string? ColumnAlias { get; set; }
}