namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCallTypeArgumentBase
{
    public abstract Result<Type> Evaluate(FunctionCallContext context);
    public abstract Result<Type> Validate(FunctionCallContext context);
    public abstract bool IsDynamic { get; }
}
