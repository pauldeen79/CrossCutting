namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface ITypedConstantArgument<out T> : IFunctionCallArgument
{
    T Value { get; }
}
