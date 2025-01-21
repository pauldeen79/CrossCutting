namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument
{
    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(Value?.GetType()!); // everything is alright for a constant value, except when it's required and the value is null
}
