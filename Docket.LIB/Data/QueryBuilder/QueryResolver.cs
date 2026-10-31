namespace Docket.LIB.Data.QueryBuilder;

public static class QueryResolver
{
    public static string ResolveOperand(Operand operand) => operand switch
    {
        Operand.OR => "OR",
        Operand.AND => "AND",
        _ => throw new InvalidOperand($"{string.Format(SystemMessages.InvalidData, "Operand")}: {operand}"),
    };

    public static string ResolveOperator(Operator opr) => opr switch
    {
        Operator.Equal => "=",
        Operator.NotEqual => "!=",
        _ => throw new InvalidOperator($"{string.Format(SystemMessages.InvalidData, "Operand")}: {opr}."),
    };

    public static string ResolveRelationtypes(RelationType relationType) => relationType switch
    {
        RelationType.InnerJoin => "INNER JOIN",
        RelationType.LeftJoin => "LEFT JOIN",
        _ => throw new InvalidRelationType($"{string.Format(SystemMessages.InvalidData, "RelationType")}: {relationType}."),
    };

    public static string ResolverOrderBy(OrderByDirection orderByDirection) => orderByDirection switch
    {
        OrderByDirection.ASC => "ASC",
        OrderByDirection.DESC => "DESC",
        _ => throw new InvalidOrderByDirection($"{string.Format(SystemMessages.InvalidData, "Operand")}: {orderByDirection}."),
    };
}