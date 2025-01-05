namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record RecursiveArgument
{
    public override Result<object?> GetValueResult(FunctionCallRequest request)
    {
        request = ArgumentGuard.IsNotNull(request, nameof(request));

        return request.FunctionEvaluator.Evaluate(Function, request.ExpressionEvaluator, request.FormatProvider, request.Context);
    }
}
