namespace CrossCutting.Data.Sql.Tests;

public abstract class TestBase<T>
{
    private T? _sut;

    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

    protected T Sut
    {
        get
        {
            if (_sut is null)
            {
                _sut = Fixture.Create<T>();
            }

            return _sut;
        }
    }
}
