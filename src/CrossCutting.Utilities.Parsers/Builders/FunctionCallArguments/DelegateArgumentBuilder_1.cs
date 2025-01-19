namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class DelegateArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new DelegateArgumentBuilder().WithDelegate(() => Delegate());

    public override FunctionCallArgument<T> BuildTyped()
        => new DelegateArgument<T>(Delegate);

    public override FunctionCallArgument Build()
        => new DelegateArgument(() => Delegate());

    public Func<T> Delegate { get; set; } = default!;

    public DelegateArgumentBuilder<T> WithDelegate(Func<T> @delegate)
    {
        Delegate = @delegate;
        return this;
    }
}
