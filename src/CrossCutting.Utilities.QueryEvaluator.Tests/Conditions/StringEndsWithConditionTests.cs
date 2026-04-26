namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringEndsWithConditionTests : TestBase<StringEndsWithCondition>
{
    public class EvaluateAsync : StringEndsWithConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "S";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = new StringEndsWithConditionBuilder()
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
            var sut = new StringEndsWithConditionBuilder()
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
            var sut = new StringEndsWithConditionBuilder()
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

    public class GetChildEvaluatables : StringEndsWithConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var sut = new StringEndsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(2);
        }
    }

    public class ToTypedBuilder : StringEndsWithConditionTests
    {
        [Fact]
        public void Returns_Valid_Builder()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new StringEndsWithConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .BuildTyped();

            // Act
            var typedBuilder = sut.ToTypedBuilder();

            // Assert
            typedBuilder.ShouldBeOfType<StringEndsWithConditionBuilder>();
        }
    }
}
