namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringEqualsConditionTests : TestBase<StringEqualsCondition>
{
    public class Evaluate : StringEqualsConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "THIS";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = new StringEqualsConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
                .WithStringComparison(StringComparison)
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
            var sut = new StringEqualsConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
                .WithStringComparison(StringComparison)
                .Build();
            var context = CreateContext();


            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }
    }
}
