namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class LiteralEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (evaluatable is ILiteralEvaluatable literalEvaluatable)
        {
            return parameterBag.CreateQueryParameterName(literalEvaluatable.GetValue());
        }      

        return Result.Continue<string>();
    }
}