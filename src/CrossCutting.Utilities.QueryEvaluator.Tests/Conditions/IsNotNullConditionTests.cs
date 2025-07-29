namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class IsNotNullConditionTests : TestBase<IsNotNullCondition>
{
    public class Evaluate : IsNotNullConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public async Task Returns_Ok_On_Null_value()
        {
            // Arrange
            var leftValue = default(object?);
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }
}
