namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateResultArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Delegate();
}
