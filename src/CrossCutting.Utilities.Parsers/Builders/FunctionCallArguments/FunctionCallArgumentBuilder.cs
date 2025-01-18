namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public abstract class FunctionCallArgumentBuilder<T> : FunctionCallArgumentBuilder
{
    public abstract FunctionCallArgument<T> BuildTyped();
}
