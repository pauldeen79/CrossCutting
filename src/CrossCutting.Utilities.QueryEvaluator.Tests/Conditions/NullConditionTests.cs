namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NullConditionTests : TestBase<NullCondition>
{
    public class Evaluate : NullConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var sut = new NullConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }

        [Fact]
        public async Task Returns_Ok_On_Null_value()
        {
            // Arrange
            var leftValue = default(object?);
            var sut = new NullConditionBuilder()
                .WithSourceExpression(new LiteralExpressionBuilder(leftValue))
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
