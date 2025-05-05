namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class PrimitiveExpressionComponentTests : TestBase<PrimitiveExpressionComponent>
{
    public class Evaluate : PrimitiveExpressionComponentTests
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void Returns_Correct_Result_For_Booleans(string expression, bool expectedValue)
        {
            // Arrange
            var context = CreateContext(expression);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(expectedValue);
        }

        [Fact]
        public void Returns_Correct_Result_For_Null()
        {
            // Arrange
            var context = CreateContext("null");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Correct_Result_For_DateTime_Now()
        {
            // Arrange
            var context = CreateContext("DateTime.Now");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(CurrentDateTime);
        }

        [Fact]
        public void Returns_Correct_Result_For_DateTime_Today()
        {
            // Arrange
            var context = CreateContext("DateTime.Today");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(CurrentDateTime.Date);
        }

        [Fact]
        public void Returns_Continue_On_Non_Primitive_Expression()
        {
            // Arrange
            var context = CreateContext("some non primitive expression");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }

    public class Parse : PrimitiveExpressionComponentTests
    {
        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void Returns_Correct_Result_For_Booleans(string expression)
        {
            // Arrange
            var context = CreateContext(expression);
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Result_For_Null()
        {
            // Arrange
            var context = CreateContext("null");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBeNull();
        }

        [Fact]
        public void Returns_Correct_Result_For_DateTime_Now()
        {
            // Arrange
            var context = CreateContext("DateTime.Now");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(DateTime));
        }

        [Fact]
        public void Returns_Correct_Result_For_DateTime_Today()
        {
            // Arrange
            var context = CreateContext("DateTime.Today");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(DateTime));
        }

        [Fact]
        public void Returns_Continue_On_Non_Primitive_Expression()
        {
            // Arrange
            var context = CreateContext("some non primitive expression");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }
}
