namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class BetweenConditionTests : TestBase<BetweenCondition>
{
    public class Evaluate : BetweenConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Integers()
        {
            // Arrange
            var sourceValue = 14;
            var lowerBoundValue = 13;
            var upperBoundValue = 15;
            var sut = new BetweenConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .WithLowerBoundExpression(new LiteralEvaluatableBuilder(lowerBoundValue))
                .WithUpperBoundExpression(new LiteralEvaluatableBuilder(upperBoundValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Invalid_On_Different_Types()
        {
            // Arrange
            var sourceValue = "this";
            var lowerBoundValue = 13;
            var upperBoundValue = 15;
            var sut = new BetweenConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .WithLowerBoundExpression(new LiteralEvaluatableBuilder(lowerBoundValue))
                .WithUpperBoundExpression(new LiteralEvaluatableBuilder(upperBoundValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }
    }
}
