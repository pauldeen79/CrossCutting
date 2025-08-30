namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryFieldInfo
{
    string? GetDatabaseFieldName(string queryFieldName);
    IEnumerable<string> GetAllFields();
}
