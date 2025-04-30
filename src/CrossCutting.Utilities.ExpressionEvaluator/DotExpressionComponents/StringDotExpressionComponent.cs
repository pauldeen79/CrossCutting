namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class StringDotExpressionComponent : DotExpressionComponentBase<string>
{
    public StringDotExpressionComponent() : base(new DotExpressionDescriptor<string>(new Dictionary<string, DotExpressionDelegates<string>>()
    {
        { "Length", new DotExpressionDelegates<string>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Length)) },
    }))
    {
    }

    public override int Order => 14;
}
