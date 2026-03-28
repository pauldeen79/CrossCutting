namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class LiteralResultEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (evaluatable is not LiteralResultEvaluatable literalResultEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return literalResultEvaluatable.Value.Status.IsSuccessful()
            ? Result.Success(parameterBag.CreateQueryParameterName(literalResultEvaluatable.Value.Value))
            : Result.FromExistingResult<string>(literalResultEvaluatable.Value);
    }
}