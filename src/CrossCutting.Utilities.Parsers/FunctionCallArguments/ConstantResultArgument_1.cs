namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantResultArgument<T>
{
    public override bool IsDynamic => false;

    public Result<T> EvaluateTyped(FunctionCallContext context)
        => Result;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Transform<object?>(value => value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Transform(x => x?.GetType()!);
}
