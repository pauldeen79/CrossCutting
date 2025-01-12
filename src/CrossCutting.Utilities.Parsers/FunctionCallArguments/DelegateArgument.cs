namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success(Delegate());
}
