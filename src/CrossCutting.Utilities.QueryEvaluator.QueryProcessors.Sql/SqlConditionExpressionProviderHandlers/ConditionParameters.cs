namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public class ConditionParameters
{
    public string Operator { get; }

    public ConditionParameters(string @operator)
    {
        ArgumentGuard.IsNotNullOrEmpty(@operator, nameof(@operator));

        Operator = @operator;
    }
}
