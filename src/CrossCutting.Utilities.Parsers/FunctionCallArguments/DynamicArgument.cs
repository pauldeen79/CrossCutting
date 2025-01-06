namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DynamicArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext request)
    {
        request = ArgumentGuard.IsNotNull(request, nameof(request));

        return request.FunctionEvaluator.Evaluate(Function, request.ExpressionEvaluator, request.FormatProvider, request.Context);
    }
}
