namespace Docket.LIB.Data.QueryBuilder.Model;

public class OrderBy
{
    public string? TableName { get; set; }
    required public string ColumnName { get; set; }
    public OrderByDirection OrderByDirection { get; set; }
}