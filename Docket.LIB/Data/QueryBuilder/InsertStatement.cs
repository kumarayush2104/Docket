using System.Text;

namespace Docket.LIB.Data.QueryBuilder;

public class InsertStatement(NpgsqlDataSource DatabaseContext) : Statement(DatabaseContext)
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

    private new void AddWhereCriteria() {}

    public override NpgsqlCommand Query()
    {
        StringBuilder query = new();
        query.Append($"INSERT INTO {TableName}( ");
        StringBuilder parameters = new();
        for (int columnCount = 0; columnCount < Columns.Count; columnCount++)
        {
            Column column = Columns[columnCount];

            query.Append(column.ColumnName);
            parameters.Append($"@COL_PARAM_{columnCount}");

            if (!string.IsNullOrEmpty(column.ColumnAlias))
                query.Append($" AS {column.ColumnAlias}");

            if (columnCount < Columns.Count - 1)
            {
                query.Append(", ");
                parameters.Append(", ");
            }
        }

        query.Append($") VALUES({parameters.ToString()})");

        query.Append(';');
        NpgsqlCommand command = new(query.ToString());
        ResolveParameters(command, Columns);
        return command;
    }
}