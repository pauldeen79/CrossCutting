namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public abstract class TypedFunctionCallArgumentBuilder<T> : FunctionCallArgumentBuilder
{
    public abstract TypedFunctionCallArgument<T> BuildTyped();
}
