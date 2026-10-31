using System.Data;
using System.Text;
using DataColumn = Docket.LIB.Data.QueryBuilder.Model.DataColumn;

namespace Docket.LIB.Data.QueryBuilder;

public abstract class Statement(NpgsqlDataSource DatabaseContext)
{
    protected string TableName = string.Empty;
    readonly protected List<WhereCriteria> WhereCriterias = [];

    public virtual void AddTableReference(RelationType relationType, string tableName, string? SourceColumn = null, string? DestinationColumn = null)
    {
        if(RelationType.Primary == relationType)
        {
            TableName = tableName;
            return;
        }
    }

    public void AddWhereCriteria(string columnName, NpgsqlDbType dataType, Operand operand = Operand.AND, string startBracket = "", string tableName = "", Operator opr = Operator.Equal, object? value = null, string endBracket = "")
    {
        WhereCriterias.Add(new WhereCriteria()
        {
            Operand = operand,
            StartBracket = startBracket,
            TableName = tableName,
            ColumnName = columnName,
            DataType = dataType,
            Operator = opr,
            Value = value,
            EndBracket = endBracket
        });
    }
    protected static string ResolveWhereCriteria(List<WhereCriteria> whereCriterias)
    {
        StringBuilder query = new();
        for (int whereCriteriaCount = 0; whereCriteriaCount < whereCriterias.Count; whereCriteriaCount++)
        {
            WhereCriteria criteria = whereCriterias[whereCriteriaCount];
            if (whereCriteriaCount <= 0)
                query.Append(" WHERE ");
            else
                query.Append($" {QueryResolver.ResolveOperand(criteria.Operand)}");

            query.Append(criteria.StartBracket);

            if (!string.IsNullOrEmpty(criteria.TableName))
                query.Append($"{criteria.TableName}.");

            query.Append(criteria.ColumnName);
            query.Append($" {QueryResolver.ResolveOperator(criteria.Operator)} ");
            query.Append($"@PARAM_{whereCriteriaCount}");
            query.Append(criteria.EndBracket);
        }

        return query.ToString();
    }
    protected static void ResolveParameters(NpgsqlCommand command, List<WhereCriteria> whereCriterias)
    {
        for (int whereCriteriaCount = 0; whereCriteriaCount < whereCriterias.Count; whereCriteriaCount++)
        {
            WhereCriteria criteria = whereCriterias[whereCriteriaCount];
            command.Parameters.AddWithValue($"PARAM_{whereCriteriaCount}", criteria.DataType, criteria.Value ?? DBNull.Value);
        }
    }

    protected static void ResolveParameters(NpgsqlCommand command, List<DataColumn> datacolumns)
    {
        for (int datacolumnCount = 0; datacolumnCount < datacolumns.Count; datacolumnCount++)
        {
            DataColumn column = datacolumns[datacolumnCount];
            command.Parameters.AddWithValue($"COL_PARAM_{datacolumnCount}", column.DataType, column.Value ?? DBNull.Value);
        }
    }

    public abstract NpgsqlCommand Query();

    public async Task ExecuteNonQuery()
    {
        using NpgsqlConnection connection = await DatabaseContext.OpenConnectionAsync();
        using NpgsqlCommand command = Query();
        command.Connection = connection;
        await command.ExecuteNonQueryAsync();
    }

    public async Task<DataTable> ExecuteQuery()
    {
        using NpgsqlConnection connection = await DatabaseContext.OpenConnectionAsync();
        using NpgsqlCommand command = Query();
        command.Connection = connection;
        using NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
        DataTable data = new();
        data.Load(dataReader);
        return data;
    }
}