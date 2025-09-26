namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class EqualConditionTests : TestBase<EqualCondition>
{
    public class Evaluate : EqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(leftValue) },
                { nameof(IDoubleExpressionContainer.CompareExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(rightValue) },
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
        public async Task Returns_Ok_On_Two_Empty_Expressions()
        {
            // Arrange
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new EmptyExpression() },
                { nameof(IDoubleExpressionContainer.CompareExpression).ToCamelCase(CultureInfo.CurrentCulture), new EmptyExpression() },
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
        public async Task Returns_Ok_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(leftValue) },
                { nameof(IDoubleExpressionContainer.CompareExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpression(rightValue) },
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
