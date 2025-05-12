namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class NumericExpressionComponentTests : TestBase<NumericExpressionComponent>
{
    public class Evaluate : NumericExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Negative_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("-13");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(-13);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Positive_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("+13");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext(((long)int.MaxValue + 1).ToString(CultureInfo.InvariantCulture));

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe((long)int.MaxValue + 1);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13L");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(13L);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.3M);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13M");

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13M);
        }
    }

    public class Parse : NumericExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Negative_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("-13");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Positive_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("+13");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext(((long)int.MaxValue + 1).ToString(CultureInfo.InvariantCulture));

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Int64()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13L");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13M");

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
