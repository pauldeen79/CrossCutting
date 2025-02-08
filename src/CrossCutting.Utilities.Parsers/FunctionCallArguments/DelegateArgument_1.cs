namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument<T>
{
    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Result.Success(Delegate());

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success<object?>(Delegate());

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(ValidationDelegate?.Invoke()!);
}
