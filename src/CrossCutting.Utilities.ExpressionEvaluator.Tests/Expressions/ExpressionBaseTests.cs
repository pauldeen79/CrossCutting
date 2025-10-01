namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class ExpressionBaseTests : TestBase
{
    public class ToBuilder : ExpressionBaseTests
    {
        ExpressionBase CreateSut() => ClassFactories.GetOrCreate<ExpressionBase>(ClassFactory);

        [Fact]
        public void Throws_NotSupportedException()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.ToBuilder();
            a.ShouldThrow<NotSupportedException>();
        }
    }
}
