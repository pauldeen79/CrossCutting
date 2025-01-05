namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext request)
        => Result.Success(default(object?));
}
