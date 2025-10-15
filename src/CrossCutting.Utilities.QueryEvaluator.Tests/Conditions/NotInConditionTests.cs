namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotInConditionTests : TestBase<NotInCondition>
{
    public class Evaluate : NotInConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "THIS";
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(rightValue))
                .Build();
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext(settings: settings);

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
            var sut = new NotInConditionBuilder()
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
    }
}
