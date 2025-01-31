namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument<T>
{
    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Result.Success(default(T)!);

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success<object?>(default);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.NoContent<Type>();
}
