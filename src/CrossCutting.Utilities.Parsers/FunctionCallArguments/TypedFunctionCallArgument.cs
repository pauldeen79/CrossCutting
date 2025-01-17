namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public abstract record TypedFunctionCallArgument<T> : FunctionCallArgument
{
    public abstract TypedFunctionCallArgumentBuilder<T> ToTypedBuilder();
}
