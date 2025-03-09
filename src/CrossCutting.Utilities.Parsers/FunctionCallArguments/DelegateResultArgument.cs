namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateResultArgument
{
    public override bool IsDynamic => true;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Delegate();

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate?.Invoke() ?? Result.NoContent<Type>();
}
