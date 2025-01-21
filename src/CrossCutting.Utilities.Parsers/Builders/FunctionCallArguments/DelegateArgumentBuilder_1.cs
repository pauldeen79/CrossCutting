namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class DelegateArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new DelegateArgumentBuilder().WithDelegate(() => Delegate()).WithValidationDelegate(ValidationDelegate);

    public override FunctionCallArgument<T> BuildTyped()
        => new DelegateArgument<T>(Delegate, ValidationDelegate);

    public override FunctionCallArgument Build()
        => new DelegateArgument(() => Delegate(), ValidationDelegate);

    public Func<T> Delegate { get; set; } = default!;
    public Func<Type>? ValidationDelegate { get; set; }

    public DelegateArgumentBuilder<T> WithDelegate(Func<T> @delegate)
    {
        Delegate = @delegate;
        return this;
    }

    public DelegateArgumentBuilder<T> WithValidationDelegate(Func<Type>? validationDelegate)
    {
        ValidationDelegate = validationDelegate;
        return this;
    }
}
