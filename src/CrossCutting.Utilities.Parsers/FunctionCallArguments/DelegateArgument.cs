namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument
{
    public override bool IsDynamic => true;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success(Delegate());

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(ValidationDelegate?.Invoke()!);
}
