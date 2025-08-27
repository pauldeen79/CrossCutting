namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class BetweenConditionTests : TestBase<BetweenCondition>
{
    public class Evaluate : BetweenConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Integers()
        {
            // Arrange
            var sourceValue = 14;
            var lowerBoundValue = 13;
            var upperBoundValue = 15;
            var parameters = new Dictionary<string, object?>
            {
                { nameof(BetweenCondition.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(sourceValue).Build() },
                { nameof(BetweenCondition.LowerBoundExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(lowerBoundValue).Build() },
                { nameof(BetweenCondition.UpperBoundExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(upperBoundValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");
            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfInvalid();
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Invalid_On_Different_Types()
        {
            // Arrange
            var sourceValue = "this";
            var lowerBoundValue = 13;
            var upperBoundValue = 15;
            var parameters = new Dictionary<string, object?>
            {
                { nameof(BetweenCondition.SourceExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(sourceValue).Build() },
                { nameof(BetweenCondition.LowerBoundExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(lowerBoundValue).Build() },
                { nameof(BetweenCondition.UpperBoundExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(upperBoundValue).Build() },
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
