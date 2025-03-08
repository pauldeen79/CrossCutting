namespace CrossCutting.Utilities.Parsers.Abstractions;

public partial interface IFunctionCallTypeArgument
{
    Result<Type> Evaluate(FunctionCallContext context);
    Result<Type> Validate(FunctionCallContext context);
    bool IsDynamic { get; }
}
