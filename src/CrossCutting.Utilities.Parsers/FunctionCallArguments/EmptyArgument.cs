namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success(default(object?));
}
