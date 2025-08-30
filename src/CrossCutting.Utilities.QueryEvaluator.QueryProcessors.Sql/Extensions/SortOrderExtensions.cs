namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

public static class SortOrderExtensions
{
    public static string ToSql(this ISortOrder instance)
        => instance.Order switch
        {
            SortOrderDirection.Ascending => "ASC",
            SortOrderDirection.Descending => "DESC",
            _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Invalid query sort order: {instance}"),
        };
}
