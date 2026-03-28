namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument<T>
{
    public override bool IsDynamic => false;

    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Value;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Value;

    public override Result<Type> Validate(FunctionCallContext context)
        => Value?.GetType()!;
}
