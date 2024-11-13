namespace CrossCutting.Data.Core;

public class DatabaseEntityRetrieverSettings(string tableName, string fields, string defaultOrderBy, string defaultWhere) : IDatabaseEntityRetrieverSettings
{
    public string TableName { get; } = tableName;
    public string Fields { get; } = fields;
    public string DefaultOrderBy { get; } = defaultOrderBy;
    public string DefaultWhere { get; } = defaultWhere;
}
