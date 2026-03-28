namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override bool IsDynamic => false;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => default(object?);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Continue<Type>();
}
