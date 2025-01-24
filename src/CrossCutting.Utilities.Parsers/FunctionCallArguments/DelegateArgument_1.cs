namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument<T> : FunctionCallArgument<T>
{
    public override Result<T> EvaluateTyped(FunctionCallContext context)
        => Result.Success(Delegate());

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success<object?>(Delegate());

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(ValidationDelegate?.Invoke()!);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new DelegateArgumentBuilder<T>().WithDelegate(Delegate);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new DelegateArgumentBuilder<T>().WithDelegate(Delegate);

    public Func<T> Delegate { get; }
    public Func<Type>? ValidationDelegate { get; }

    public DelegateArgument(Func<T> @delegate, Func<Type>? validationDelegate)
    {
        ArgumentGuard.IsNotNull(@delegate, nameof(@delegate));

        Delegate = @delegate;
        ValidationDelegate = validationDelegate;
    }
}
