namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallArguments;

internal interface IDelegateArgument : IFunctionCallArgumentBase
{
    [Required] Func<object?> Delegate { get; }
    Func<Type>? ValidationDelegate { get; }
}

internal interface IDelegateArgument<T> : IFunctionCallArgumentBase, Abstractions.IFunctionCallArgument<T>
{
    [Required] Func<T> Delegate { get; }
    Func<Type>? ValidationDelegate { get; }
}
