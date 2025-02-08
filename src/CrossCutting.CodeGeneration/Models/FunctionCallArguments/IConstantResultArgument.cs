namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IConstantResultArgument : IFunctionCallArgumentBase
{
    [Required] Result<object?> Result { get; }
}

internal interface IConstantResultArgument<T> : IFunctionCallArgumentBase, Abstractions.IFunctionCallArgument<T>
{
    Result<T> Result { get; }
}
