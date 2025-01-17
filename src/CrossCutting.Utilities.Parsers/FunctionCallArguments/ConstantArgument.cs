namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success(Value);
}
