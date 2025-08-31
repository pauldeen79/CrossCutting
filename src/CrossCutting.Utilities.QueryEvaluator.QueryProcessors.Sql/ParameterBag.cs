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

    internal Result<string> ReplaceString(string key, string formatString)
    {
        var found = _parameters
            .Select(x => new { Item = x })
            .FirstOrDefault(x => x.Item.Key == key);

        if (found is null)
        {
            return Result.Invalid<string>("There are no parameters");
        }

        if (formatString != "{0}")
        {
            // For non-default format strings, replace the value
            var newValue = string.Format(formatString, found.Item.Value);
            _parameters.Remove(_parameters[_paramCounter - 1]);
            _parameters.Add(new KeyValuePair<string, object?>(found.Item.Key, newValue));
        }

        return Result.Success(found.Item.Key);
    }

    private string Add(string key, object? value)
    {
        _parameters.Add(new KeyValuePair<string, object?>(key, value));
        return key;
    }
}
