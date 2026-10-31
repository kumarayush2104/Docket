namespace Docket.LIB.Data.QueryBuilder.Model;
public class WhereCriteria
{
    public Operand Operand { get; set; } = Operand.AND;
    required public string ColumnName { get; set; }
    public string? TableName { get; set; }
    public Operator Operator { get; set; } = Operator.Equal;
    required public NpgsqlDbType DataType { get; set; }
    public object? Value { get; set; }
    public string StartBracket { get; set; } = string.Empty;
    public string EndBracket { get; set; } = string.Empty;
}