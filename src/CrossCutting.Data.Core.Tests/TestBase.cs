using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace CrossCutting.Data.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase<T>
    {
        protected IFixture Fixture { get; } = new Fixture().Customize(new AutoMoqCustomization());
        protected T Sut => Fixture.Create<T>();
    }
}
