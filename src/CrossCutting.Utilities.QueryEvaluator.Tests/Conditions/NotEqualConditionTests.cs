namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotEqualConditionTests : TestBase<NotEqualCondition>
{
    public class Evaluate : NotEqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }

        [Fact]
        public async Task Returns_Ok_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }
}
