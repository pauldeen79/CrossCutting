namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class SmallerThanOrEqualConditionTests : TestBase<SmallerThanOrEqualCondition>
{
    public class Evaluate : SmallerThanOrEqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = 13;
            var rightValue = 15;
            var sut = new SmallerThanOrEqualConditionBuilder()
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

        [Fact]
        public async Task Returns_Invalid_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            var sut = new SmallerThanOrEqualConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
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
