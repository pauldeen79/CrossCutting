namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DelegateArgument<T> : FunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result.Success(Delegate());

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success<object?>(Delegate());

    public override FunctionCallArgumentBuilder ToBuilder()
        => new DelegateArgumentBuilder<T>().WithDelegate(Delegate);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new DelegateArgumentBuilder<T>().WithDelegate(Delegate);

    public Func<T> Delegate { get; }

    public DelegateArgument(Func<T> @delegate)
    {
        Delegate = @delegate;
    }
}
