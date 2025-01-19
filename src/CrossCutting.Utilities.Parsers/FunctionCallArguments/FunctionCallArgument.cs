namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public abstract record FunctionCallArgument<T> : FunctionCallArgument
{
    public abstract FunctionCallArgumentBuilder<T> ToTypedBuilder();
}
