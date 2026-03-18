namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class BinaryOrOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not BinaryOrOperatorEvaluatable binaryOrOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }      
        
        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(BinaryOrOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, binaryOrOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(BinaryOrOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, binaryOrOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "OR"));
    }
}