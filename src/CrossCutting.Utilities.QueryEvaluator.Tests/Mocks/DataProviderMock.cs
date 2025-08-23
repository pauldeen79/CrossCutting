namespace CrossCutting.Utilities.QueryEvaluator.Tests.Mocks;

internal sealed class DataProviderMock : DataProviderBase
{
    private readonly Func<IEnumerable<object>> _sourceDataDelegate;

    public DataProviderMock(IExpressionEvaluator expressionEvaluator, Func<IEnumerable<object>> sourceDataDelegate) : base(expressionEvaluator) //base(Substitute.For<IExpressionEvaluator>())
    {
        _sourceDataDelegate = sourceDataDelegate;
    }

    public override IEnumerable<object> GetSourceData() => _sourceDataDelegate();
}
