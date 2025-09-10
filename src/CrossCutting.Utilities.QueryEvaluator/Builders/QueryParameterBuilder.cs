namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders;

public partial class QueryParameterBuilder
{
    public QueryParameterBuilder(string name, object? value)
    {
        _name = name;
        _value = value;
    }
}
