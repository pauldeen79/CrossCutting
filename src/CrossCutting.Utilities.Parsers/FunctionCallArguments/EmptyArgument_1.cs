namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument<T>
{
    public override bool IsDynamic => false;

    public Result<T> EvaluateTyped(FunctionCallContext context)
        => default(T)!;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.NoContent<object?>();

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Continue<Type>();
}
