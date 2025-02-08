namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument<T>
{
    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success<object?>(Value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(Value?.GetType()!);
}
