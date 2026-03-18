namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class BinaryAndOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not BinaryAndOperatorEvaluatable binaryAndOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(BinaryAndOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, binaryAndOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(BinaryAndOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, binaryAndOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND"));
    }
}