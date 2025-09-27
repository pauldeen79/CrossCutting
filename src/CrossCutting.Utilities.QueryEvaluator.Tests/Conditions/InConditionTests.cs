namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class InConditionTests : TestBase<InCondition>
{
    public class Evaluate : InConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .AddCompareExpressions(new LiteralExpressionBuilder(rightValue))
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
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .AddCompareExpressions(rightValue.Select(x => new LiteralExpressionBuilder(x)))
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
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .AddCompareExpressions(new LiteralExpressionBuilder(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }
    }
}
