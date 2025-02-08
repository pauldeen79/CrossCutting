namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IDelegateResultArgument : IFunctionCallArgumentBase
{
    // using CsharpTypeName here to fix bug in generating nullable annotation on System.Object nested type argument...
    [Required][CsharpTypeName("System.Func<CrossCutting.Common.Results.Result<System.Object?>>")] Func<Result<object?>> Delegate { get; }
    Func<Result<Type>>? ValidationDelegate { get; }
}

internal interface IDelegateResultArgument<T> : IFunctionCallArgumentBase, Abstractions.IFunctionCallArgument<T>
{
    [Required] Func<Result<T>> Delegate { get; }
    Func<Result<Type>>? ValidationDelegate { get; }
}
