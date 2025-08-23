namespace CrossCutting.Utilities.QueryEvaluator.Tests.Mocks;

internal sealed class MyEntity
{
    public MyEntity(string property1, string property2)
    {
        Property1 = property1;
        Property2 = property2;
    }

    public string Property1 { get; }
    public string Property2 { get; }
}

internal sealed class MyNestedEntity
{
    public MyNestedEntity(MyEntity property)
    {
        Property = property;
    }

    public MyEntity Property { get; }
}
