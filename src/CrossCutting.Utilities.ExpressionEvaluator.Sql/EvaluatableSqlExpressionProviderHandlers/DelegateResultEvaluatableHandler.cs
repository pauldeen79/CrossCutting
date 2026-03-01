namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class DelegateResultEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (evaluatable is not DelegateResultEvaluatable delegateResultEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return Result.WrapException(() => {
            var result = delegateResultEvaluatable.Value();
            return result.Status.IsSuccessful()
                ? Result.Success(parameterBag.CreateQueryParameterName(result.Value))
                : Result.FromExistingResult<string>(result);
        });
    }
}