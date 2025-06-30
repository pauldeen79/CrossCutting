namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class IsNullConditionTests : TestBase<IsNullCondition>
{
    public class Evaluate : IsNullConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder().WithValue(leftValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }

        [Fact]
        public async Task Returns_Ok_On_Null_value()
        {
            // Arrange
            var leftValue = default(object?);
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder().WithValue(leftValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }
}
