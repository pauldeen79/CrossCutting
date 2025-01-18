namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateResultArgument<T> : FunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Delegate();

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Delegate().Transform<object?>(value => value);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new DelegateResultArgumentBuilder<T>().WithDelegate(Delegate);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new DelegateResultArgumentBuilder<T>().WithDelegate(Delegate);

    public Func<Result<T>> Delegate { get; }

    public DelegateResultArgument(Func<Result<T>> @delegate)
    {
        Delegate = @delegate;
    }
}
