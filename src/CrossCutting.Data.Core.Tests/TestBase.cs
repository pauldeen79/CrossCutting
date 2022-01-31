namespace CrossCutting.Data.Core.Tests;

public abstract class TestBase<T>
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoMoqCustomization());
    protected T Sut => Fixture.Create<T>();
}
