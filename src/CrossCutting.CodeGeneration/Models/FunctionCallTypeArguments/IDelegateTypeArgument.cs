namespace CrossCutting.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IDelegateTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required]Func<Type> Delegate{ get; }
    Func<Type>? ValidationDelegate { get; }
}
