namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantResultArgument
{
    public override bool IsDynamic => false;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result;

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Transform(x => x?.GetType()!);
}
