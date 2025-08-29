namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotEqualConditionTests : TestBase<NotEqualCondition>
{
    public class Evaluate : NotEqualConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
                { nameof(IDoubleExpressionContainer.CompareExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(rightValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");


            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfInvalid();
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
                { nameof(IDoubleExpressionContainer.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
                { nameof(IDoubleExpressionContainer.CompareExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(rightValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");


            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfInvalid();
            result.Value.ShouldBe(true);
        }
    }
}
