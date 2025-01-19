namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class DelegateResultArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new DelegateResultArgumentBuilder().WithDelegate(() => Delegate().Transform<object?>(value => value));

    public override FunctionCallArgument<T> BuildTyped()
        => new DelegateResultArgument<T>(Delegate);

    public override FunctionCallArgument Build()
        => new DelegateResultArgument(() => Delegate().Transform<object?>(value => value));

    public Func<Result<T>> Delegate { get; set; } = default!;

    public DelegateResultArgumentBuilder<T> WithDelegate(Func<Result<T>> @delegate)
    {
        Delegate = @delegate;
        return this;
    }
}
