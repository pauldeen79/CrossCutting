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

        [Fact]
        public async Task Returns_Ok_On_String_And_String_Array()
        {
            // Arrange
            var leftValue = "a";
            var rightValue = new List<string> { "A", "B", "C" };
            var parameters = new Dictionary<string, object?>
            {
                { nameof(InCondition.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(leftValue) },
                { nameof(InCondition.CompareExpressions).ToCamelCase(CultureInfo.CurrentCulture), rightValue.Select(x => new LiteralExpression(x)).ToList() },
            };
            var sut = CreateSut(parameters);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext("Dummy", settings: settings);


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
            result.Value.ShouldBe(false);
        }
    }
}
