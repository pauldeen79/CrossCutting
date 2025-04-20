namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class StringExpressionComponentTests : TestBase<StringExpressionComponent>
{
    public class Evaluate : StringExpressionComponentTests
    {
        [Fact]
        public void Returns_Success_On_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("\"my string value\"");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("my string value");
        }

        [Fact]
        public void Returns_Continue_On_Non_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("null");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }

    public class Parse : StringExpressionComponentTests
    {
        [Fact]
        public void Returns_Success_On_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("\"my string value\"");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(string));
            result.ExpressionComponentType.ShouldBe(typeof(StringExpressionComponent));
        }

        [Fact]
        public void Returns_Continue_On_Non_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("null");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.ExpressionComponentType.ShouldBe(typeof(StringExpressionComponent));
        }
    }
}
