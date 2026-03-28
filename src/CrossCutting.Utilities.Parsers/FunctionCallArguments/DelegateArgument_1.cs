namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument<T>
{
    public override bool IsDynamic => true;

    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Delegate();

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Delegate();

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate?.Invoke()!;
}
