namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class DelegateEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (evaluatable is not DelegateEvaluatable delegateEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return Result.WrapException(() => parameterBag.CreateQueryParameterName(delegateEvaluatable.Value()));
    }
}