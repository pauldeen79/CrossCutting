namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCallArgument
{
    public abstract Result<object?> Evaluate(FunctionCallContext context);
    public abstract Result<Type> Validate(FunctionCallContext context);
}
