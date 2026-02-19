namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryFieldInfo : IFieldNameProvider
{
    IEnumerable<string> GetAllFields();
}
