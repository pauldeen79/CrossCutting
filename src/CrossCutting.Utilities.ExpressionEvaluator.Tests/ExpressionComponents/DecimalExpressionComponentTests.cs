namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class DecimalExpressionComponentTests : TestBase<DecimalExpressionComponent>
{
    public class Evaluate : DecimalExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

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
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13M);
        }
    }

    public class Parse : DecimalExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Decimal()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("1.3");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

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
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
