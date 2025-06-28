namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IDelegateResultTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Func<Result<Type>> Delegate { get; }
    Func<Result<Type>>? ValidationDelegate { get; }
}
