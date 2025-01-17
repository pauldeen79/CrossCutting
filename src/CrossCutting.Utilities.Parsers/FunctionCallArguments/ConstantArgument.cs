namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return Result.Success(Value);
    }
}
