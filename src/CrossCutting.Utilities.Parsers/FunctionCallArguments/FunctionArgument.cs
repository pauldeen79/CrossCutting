namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record RecursiveArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider)
    {
        functionEvaluator = ArgumentGuard.IsNotNull(functionEvaluator, nameof(functionEvaluator));

        return functionEvaluator.Evaluate(Function, expressionEvaluator, formatProvider, context);
    }
}
