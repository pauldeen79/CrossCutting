namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record RecursiveArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        evaluator = ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        return evaluator.Evaluate(Function, parser, context);
    }

    public override Result ValidateValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        evaluator = ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        return evaluator.Validate(Function, parser, context);
    }
}
