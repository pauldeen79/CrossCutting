namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class LiteralResultEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (evaluatable is not ILiteralResultEvaluatable literalResultEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return literalResultEvaluatable.GetValue().Status.IsSuccessful()
            ? parameterBag.CreateQueryParameterName(literalResultEvaluatable.GetValue().GetValue())
            : Result.FromExistingResult<string>(literalResultEvaluatable.GetValue());
    }
}