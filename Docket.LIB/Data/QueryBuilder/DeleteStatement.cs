using System.Text;

namespace Docket.LIB.Data.QueryBuilder;

public class DeleteStatement(NpgsqlDataSource DatabaseContext) : Statement(DatabaseContext)
{
    public override NpgsqlCommand Query()
    {
        StringBuilder query = new();
        query.Append($"DELETE FROM {TableName} ");
        query.Append(ResolveWhereCriteria(WhereCriterias));
        query.Append(';');
        NpgsqlCommand command = new(query.ToString());
        ResolveParameters(command, WhereCriterias);
        return command;
    }
}