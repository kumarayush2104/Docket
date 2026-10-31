namespace Docket.LIB.Data.QueryBuilder.Model;

public class DataColumn: Column
{
    public object? Value { get; set; }
    required public NpgsqlDbType DataType { get; set; }
}