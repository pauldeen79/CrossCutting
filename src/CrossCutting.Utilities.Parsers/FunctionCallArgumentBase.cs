namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCallArgumentBase
{
    public abstract Result<object?> Evaluate(FunctionCallContext context);
    public abstract Result<Type> Validate(FunctionCallContext context);
    public abstract bool IsDynamic { get; }
}
