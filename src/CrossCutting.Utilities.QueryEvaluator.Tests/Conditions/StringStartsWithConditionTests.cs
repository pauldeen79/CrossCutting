namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringStartsWithConditionTests : TestBase<StringStartsWithCondition>
{
    public class EvaluateAsync : StringStartsWithConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "T";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = new StringStartsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
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
            var sut = new StringStartsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();

            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftOperand is not of type string");
        }

        [Fact]
        public async Task Returns_Invalid_On_Right_No_String()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var sut = new StringStartsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();

            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("RightOperand is not of type string");
        }
    }

    public class GetChildEvaluatables : StringStartsWithConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var sut = new StringStartsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(2);
        }
    }
}
