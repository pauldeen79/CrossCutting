namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantResultArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result;
}
