namespace Docket.LIB.Data.QueryBuilder.Model;

public class TableReference
{
    required public string TableName { get; set; }
    required public RelationType RelationType { get; set; }
    required public string SourceColumn { get; set; }
    required public string DestinationColumn { get; set; }
}