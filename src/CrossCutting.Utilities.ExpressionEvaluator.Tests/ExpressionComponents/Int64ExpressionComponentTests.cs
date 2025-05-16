namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class Int64ExpressionComponentTests : TestBase<Int64ExpressionComponent>
{
    public class Evaluate : Int64ExpressionComponentTests
    {
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
    }

    public class Parse : Int64ExpressionComponentTests
    {
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
    }
}
