namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext request)
    {
        request = ArgumentGuard.IsNotNull(request, nameof(request));

        var result = request.ExpressionEvaluator.Evaluate(Value, request.FormatProvider, request.Context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Value)
            : result;
    }
}
