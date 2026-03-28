namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument
{
    public override bool IsDynamic => true;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Delegate();

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate?.Invoke()!;
}
