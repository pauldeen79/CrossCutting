namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class GreaterThanConditionTests : TestBase<GreaterThanCondition>
{
    public class Evaluate : GreaterThanConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = 15;
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
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Invalid_On_Different_Types()
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
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }
    }
}
