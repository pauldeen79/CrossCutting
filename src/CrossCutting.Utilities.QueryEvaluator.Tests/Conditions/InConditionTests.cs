namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class InConditionTests : TestBase<InCondition>
{
    public class EvaluateAsync : InConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var sourceValue = "this";
            var compareValues = "this";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValues))
                .Build();
            var context = CreateContext();

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
            var sourceValue = "a";
            var compareValues = new List<string> { "A", "B", "C" };
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(compareValues.Select(x => new LiteralEvaluatableBuilder(x)))
                .Build();
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext(settings: settings);

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
            var sourceValue = "this";
            var compareValues = 13;
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValues))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : InConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var sourceValue = "this";
            var compareValues = "this";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValues))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeTrue();
        }
    }

    public class GetChildEvaluatables : InConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var sourvceValue = "this";
            var compareValue1 = "this1";
            var compareValue2 = "this2";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourvceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue1), new LiteralEvaluatableBuilder(compareValue2))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(3);
        }
    }

    public class ToTypedBuilder : InConditionTests
    {
        [Fact]
        public void Returns_Valid_Builder()
        {
            // Arrange
            var sourvceValue = "this";
            var compareValue1 = "this1";
            var compareValue2 = "this2";
            var sut = new InConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourvceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue1), new LiteralEvaluatableBuilder(compareValue2))
                .Build();

            // Act
            var typedBuilder = sut.ToTypedBuilder();

            // Assert
            typedBuilder.ShouldBeOfType<InConditionBuilder>();
        }
    }
}
