namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateResultArgument<T> : FunctionCallArgument<T>
{
    public override Result<T> EvaluateTyped(FunctionCallContext context)
        => Delegate();

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Delegate().Transform<object?>(value => value);

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate?.Invoke() ?? Result.Success<Type>(default!);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new DelegateResultArgumentBuilder<T>().WithDelegate(Delegate);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new DelegateResultArgumentBuilder<T>().WithDelegate(Delegate);

    public Func<Result<T>> Delegate { get; }
    public Func<Result<Type>>? ValidationDelegate { get; }

    public DelegateResultArgument(Func<Result<T>> @delegate, Func<Result<Type>>? validationDelegate)
    {
        ArgumentGuard.IsNotNull(@delegate, nameof(@delegate));

        Delegate = @delegate;
        ValidationDelegate = validationDelegate;
    }
}
