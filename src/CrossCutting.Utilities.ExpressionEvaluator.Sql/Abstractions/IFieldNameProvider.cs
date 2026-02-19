namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IFieldNameProvider
{
    string? GetDatabaseFieldName(string queryFieldName);
}
