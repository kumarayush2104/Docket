using System.Text;

namespace Docket.LIB.Data.QueryBuilder;

public class UpdateStatement(NpgsqlDataSource DatabaseContext) : Statement(DatabaseContext)
{
    readonly private List<DataColumn> Columns = [];

    public void AddColumn(string columnName, NpgsqlDbType datatype, object? value = null)
    {
        Columns.Add(new DataColumn()
        {
            ColumnName = columnName,
            DataType = datatype,
            Value = value
        });
    }

    public override NpgsqlCommand Query()
    {
        StringBuilder query = new();
        query.Append($"UPDATE {TableName} SET ");
        for (int columnCount = 0; columnCount < Columns.Count; columnCount++)
        {
            Column column = Columns[columnCount];

            query.Append($"{column.ColumnName} = @COL_PARAM_{columnCount}");
            if (columnCount < Columns.Count - 1)
            {
                query.Append(", ");
            }
        }
        query.Append(ResolveWhereCriteria(WhereCriterias));
        query.Append(';');
        NpgsqlCommand command = new(query.ToString());
        ResolveParameters(command, Columns);
        ResolveParameters(command, WhereCriterias);
        return command;
    }
}