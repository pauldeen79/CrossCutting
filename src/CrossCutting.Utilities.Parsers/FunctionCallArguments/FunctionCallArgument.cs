namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public abstract record FunctionCallArgument<T> : FunctionCallArgument
{
    public abstract Result<T> EvaluateTyped(FunctionCallContext context);
    public abstract FunctionCallArgumentBuilder<T> ToTypedBuilder();
}
