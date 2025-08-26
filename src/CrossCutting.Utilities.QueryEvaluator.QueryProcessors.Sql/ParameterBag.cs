namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class ParameterBag
{
    private readonly List<KeyValuePair<string, object?>> _parameters = new List<KeyValuePair<string, object?>>();
    private int _paramCounter;

    public IReadOnlyCollection<KeyValuePair<string, object?>> Parameters => _parameters.AsReadOnly();

    public string CreateQueryParameterName(object? value)
    {
        if (value is KeyValuePair<string, object> keyValuePair)
        {
            return Add($"@{keyValuePair.Key}", value);
        }

        if (value is IQueryParameterValue queryParameterValue)
        {
            return Add($"@{queryParameterValue.Name}", value);
        }

        var returnValue = Add($"@p{_paramCounter}", value);
        _paramCounter++;
        return returnValue;
    }

    private string Add(string key, object? value)
    {
        _parameters.Add(new KeyValuePair<string, object?>(key, value));
        return key;
    }
}
