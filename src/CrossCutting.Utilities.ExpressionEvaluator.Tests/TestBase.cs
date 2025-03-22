namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase<T> where T : new()
{
    protected static T CreateSut() => new T();
}
