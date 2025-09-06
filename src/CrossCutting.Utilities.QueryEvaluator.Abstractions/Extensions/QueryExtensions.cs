namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Extensions;

public static class QueryExtensions
{
    public static string GetTableName(this IQuery instance, string tableName)
        => instance is IDataObjectNameQuery dataObjectNameQuery && !string.IsNullOrEmpty(dataObjectNameQuery.DataObjectName)
            ? dataObjectNameQuery.DataObjectName
            : tableName;
}
