namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotEqualConditionTests : TestBase<NotEqualCondition>
{
    public class EvaluateAsync : NotEqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
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
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : NotEqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeFalse();
        }
    }

    public class GetChildEvaluatables : NotEqualConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(2);
        }
    }
    
    public class ToTypedBuilder : NotEqualConditionTests
    {
        [Fact]
        public void Returns_Valid_Builder()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new NotEqualConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder(rightValue))
                .BuildTyped();

            // Act
            var typedBuilder = sut.ToTypedBuilder();

            // Assert
            typedBuilder.ShouldBeOfType<NotEqualConditionBuilder>();
        }
    }
}
