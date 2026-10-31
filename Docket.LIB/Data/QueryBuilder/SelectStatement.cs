using System.Text;

namespace Docket.LIB.Data.QueryBuilder;

public class SelectStatement(NpgsqlDataSource DatabaseContext) : Statement(DatabaseContext)
{
    readonly private List<Column> Columns = [];
    readonly private List<TableReference> TableReferences = [];
    readonly private List<OrderBy> OrderBy = [];

    public void AddColumn(string columnName, string? tableName = null, string? columnAlias = null)
    {
        Columns.Add(new Column()
        {
            ColumnName = columnName,
            TableName = tableName,
            ColumnAlias = columnAlias
        });
    }

    public override void AddTableReference(RelationType relationType, string tableName, string? sourceColumn = null, string? destinationColumn = null)
    {
        base.AddTableReference(relationType, tableName, sourceColumn, destinationColumn);
        if(relationType == RelationType.Primary) return;
        TableReferences.Add(new TableReference()
        {
           TableName = tableName,
           RelationType = relationType,
           SourceColumn = sourceColumn ?? string.Empty,
           DestinationColumn = destinationColumn ?? string.Empty
        });
    }

    public override NpgsqlCommand Query()
    {
        StringBuilder query = new();
        query.Append("SELECT ");
        for (int columnCount = 0; columnCount < Columns.Count; columnCount++)
        {
            Column column = Columns[columnCount];
            if(!string.IsNullOrEmpty(column.TableName))
                query.Append($"{column.TableName}.");

            query.Append(column.ColumnName);

            if(!string.IsNullOrEmpty(column.ColumnAlias))
                query.Append($" AS {column.ColumnAlias}");

            if(columnCount < Columns.Count - 1)
                query.Append(", ");
        }
        query.Append($" FROM {TableName}");

        foreach(TableReference tableReference in TableReferences)
        {
            query.Append($" {QueryResolver.ResolveRelationtypes(tableReference.RelationType)} {tableReference.TableName} ON {TableName}.{tableReference.SourceColumn} = {tableReference.TableName}.{tableReference.DestinationColumn}");
        }

        query.Append(ResolveWhereCriteria(WhereCriterias));

        if(OrderBy.Count > 0)
            query.Append(" ORDER BY ");

        for (int ordercount = 0; ordercount < OrderBy.Count; ordercount++)
        {
            OrderBy orderCriteria = OrderBy[ordercount];
            if(!string.IsNullOrEmpty(orderCriteria.TableName))
                query.Append($"{orderCriteria.TableName}.");

            query.Append($"{orderCriteria.ColumnName} {QueryResolver.ResolverOrderBy(orderCriteria.OrderByDirection)}");

            if(ordercount < OrderBy.Count - 1)
                query.Append(", ");
        }

        query.Append(';');
        NpgsqlCommand command = new(query.ToString());
        ResolveParameters(command, WhereCriterias);
        return command;
    }
}