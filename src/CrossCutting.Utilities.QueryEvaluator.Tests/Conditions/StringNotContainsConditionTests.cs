namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotContainsConditionTests : TestBase<StringNotContainsCondition>
{
    public class Evaluate : StringNotContainsConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "a";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = new StringNotContainsConditionBuilder()
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
        public async Task Returns_Invalid_On_Left_No_String()
        {
            // Arrange
            var leftValue = 1;
            var rightValue = "this";
            var sut = new StringNotContainsConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }

        [Fact]
        public async Task Returns_Invalid_On_Right_No_String()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var sut = new StringNotContainsConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .WithCompareExpression(new LiteralExpressionBuilder(rightValue))
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
