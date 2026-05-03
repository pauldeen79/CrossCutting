namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotInConditionTests : TestBase<NotInCondition>
{
    public class EvaluateAsync : NotInConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var sourceValue = "this";
            var compareValue = "THIS";
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue))
                .Build();
            var settings = new ExpressionEvaluatorSettingsBuilder().WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext(settings: settings);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(false);
        }

        [Fact]
        public async Task Returns_Ok_On_Different_Types()
        {
            // Arrange
            var sourceValue = "this";
            var compareValue = 13;
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue))
                .Build();
            var context = CreateContext();


            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : NotInConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var sourceValue = "this";
            var compareValue = "THIS";
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue))
                .Build();
            var settings = new ExpressionEvaluatorSettingsBuilder()
                .WithStringComparison(StringComparison.OrdinalIgnoreCase);
            var context = CreateContext(settings: settings);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBeFalse();
        }
    }

    public class GetChildEvaluatables : NotInConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var sourceValue = "this";
            var compareValue1 = "this1";
            var compareValue2 = "this2";
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue1), new LiteralEvaluatableBuilder(compareValue2))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(3);
        }
    }
    
    public class ToTypedBuilder : NotInConditionTests
    {
        [Fact]
        public void Returns_Valid_Builder()
        {
            // Arrange
            var sourvceValue = "this";
            var compareValue1 = "this1";
            var compareValue2 = "this2";
            var sut = new NotInConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder(sourvceValue))
                .AddCompareExpressions(new LiteralEvaluatableBuilder(compareValue1), new LiteralEvaluatableBuilder(compareValue2))
                .Build();

            // Act
            var typedBuilder = sut.ToTypedBuilder();

            // Assert
            typedBuilder.ShouldBeOfType<NotInConditionBuilder>();
        }
    }
}
