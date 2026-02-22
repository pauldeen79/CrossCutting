namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class InConditionTests : TestBase<InCondition>
{
    public class EvaluateAsync : InConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Ok_On_String_And_String_Array()
        {
            // Arrange
            var leftValue = "a";
            var rightValue = new List<string> { "A", "B", "C" };
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(rightValue.Select(x => new LiteralEvaluatableBuilder(x)))
                .Build();
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext(settings: settings);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Ok_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : InConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeTrue();
        }
    }

    public class GetChildEvaluatables : InConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var leftValue = "this";
            var rightValue1 = "this1";
            var rightValue2 = "this2";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(rightValue1), new LiteralEvaluatableBuilder(rightValue2))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(3);
        }
    }
}
