namespace CrossCutting.Utilities.Parsers.Contracts;

public interface ITypedFunctionCallArgumentBuilder<T>
{
    ITypedFunctionCallArgument<T> Build();
    FunctionCallArgumentBuilder ToUntyped();
}
