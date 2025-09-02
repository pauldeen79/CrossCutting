namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public class StringConditionParameters : ConditionParameters
{
    public string FormatString { get; }

    public StringConditionParameters(string @operator, string formatString) : base(@operator)
    {
        ArgumentGuard.IsNotNullOrEmpty(formatString, nameof(formatString));

        FormatString = formatString;
    }
}
