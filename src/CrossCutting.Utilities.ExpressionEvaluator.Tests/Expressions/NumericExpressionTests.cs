namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class NumericExpressionTests : TestBase<NumericExpression>
{
    public class Evaluate : NumericExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }

        [Fact]
        public void Returns_Correct_Result_For_Negative_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("-13");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(-13);
        }

        [Fact]
        public void Returns_Correct_Result_For_Forced_Positive_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("+13");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }

        [Fact]
        public void Returns_Correct_Result_For_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext(((long)int.MaxValue + 1).ToString(CultureInfo.InvariantCulture));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe((long)int.MaxValue + 1);
        }

        [Fact]
        public void Returns_Correct_Result_For_Forced_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13L");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(13L);
        }

        [Fact]
        public void Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.3M);
        }

        [Fact]
        public void Returns_Correct_Result_For_Forced_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13M");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13M);
        }
    }

    public class Parse : NumericExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_For_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext(((long)int.MaxValue + 1).ToString(CultureInfo.InvariantCulture));

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_For_Forced_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13L");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_For_Forced_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13M");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
