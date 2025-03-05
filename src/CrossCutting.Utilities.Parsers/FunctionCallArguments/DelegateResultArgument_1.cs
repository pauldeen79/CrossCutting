namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateResultArgument<T>
{
    public override bool IsDynamic => true;

    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Delegate();

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Delegate().Transform<object?>(value => value);

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate?.Invoke() ?? Result.NoContent<Type>();
}
