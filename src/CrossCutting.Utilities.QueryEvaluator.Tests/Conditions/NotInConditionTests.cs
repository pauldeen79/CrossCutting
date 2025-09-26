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
            var parameters = new Dictionary<string, object?>
            {
                { nameof(InCondition.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(leftValue) },
                { nameof(InCondition.CompareExpressions).ToCamelCase(CultureInfo.CurrentCulture), new List<IExpression> { new LiteralExpression(rightValue) } },
            };
            var sut = CreateSut(parameters);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext("Dummy", settings: settings);


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
            var parameters = new Dictionary<string, object?>
            {
                { nameof(InCondition.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(leftValue) },
                { nameof(InCondition.CompareExpressions).ToCamelCase(CultureInfo.CurrentCulture), new List<IExpression> { new LiteralExpression(rightValue) } },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");


            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }
}
