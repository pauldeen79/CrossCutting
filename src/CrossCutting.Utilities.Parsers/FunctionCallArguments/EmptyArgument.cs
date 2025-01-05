namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override Result<object?> GetValueResult(FunctionCallRequest request)
        => Result.Success(default(object?));
}
