namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class DelegateResultArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new DelegateResultArgumentBuilder().WithDelegate(() => Delegate().Transform<object?>(value => value)).WithValidationDelegate(ValidationDelegate);

    public override FunctionCallArgument<T> BuildTyped()
        => new DelegateResultArgument<T>(Delegate, ValidationDelegate);

    public override FunctionCallArgument Build()
        => new DelegateResultArgument(() => Delegate().Transform<object?>(value => value), ValidationDelegate);

    public Func<Result<T>> Delegate { get; set; } = default!;
    public Func<Result<Type>>? ValidationDelegate { get; set; }

    public DelegateResultArgumentBuilder<T> WithDelegate(Func<Result<T>> @delegate)
    {
        Delegate = @delegate;
        return this;
    }

    public DelegateResultArgumentBuilder<T> WithValidationDelegate(Func<Result<Type>>? validationDelegate)
    {
        ValidationDelegate = validationDelegate;
        return this;
    }
}
