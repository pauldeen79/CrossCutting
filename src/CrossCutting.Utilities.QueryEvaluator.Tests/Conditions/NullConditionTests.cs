namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NullConditionTests : TestBase<NullCondition>
{
    public class EvaluateAsync : NullConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var sut = new NullConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
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
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : NullConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var sut = new NullConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeFalse();
        }

        [Fact]
        public async Task Returns_Ok_On_Null_value()
        {
            // Arrange
            var leftValue = default(object?);
            var sut = new NullConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(leftValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeTrue();
        }
    }
}
