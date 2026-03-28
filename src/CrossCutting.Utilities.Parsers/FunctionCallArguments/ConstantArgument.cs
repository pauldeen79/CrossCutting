namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument
{
    public override bool IsDynamic => false;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Value;

    public override Result<Type> Validate(FunctionCallContext context)
        => Value?.GetType()!; // everything is alright for a constant value, except when it's required and the value is null
}
