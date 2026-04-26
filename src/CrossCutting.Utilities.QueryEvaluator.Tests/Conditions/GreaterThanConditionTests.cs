namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class GreaterThanConditionTests : TestBase<GreaterThanCondition>
{
    public class EvaluateAsync : GreaterThanConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Integers()
        {
            // Arrange
            var leftValue = 15;
            var rightValue = 13;
            var sut = new GreaterThanConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder<int>(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder<int>(rightValue))
                .Build();
            var context = CreateContext();

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
            var sut = new GreaterThanConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder<string>(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder<int>(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }
    }

    public class EvaluateTypedAsync : GreaterThanConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Integers()
        {
            // Arrange
            var leftValue = 15;
            var rightValue = 13;
            var sut = new GreaterThanConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder<int>(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder<int>(rightValue))
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.ThrowIfNotSuccessful();
            result.Value.ShouldBe(true);
        }
    }

    public class GetChildEvaluatables : GreaterThanConditionTests
    {
        [Fact]
        public void Returns_Child_Evaluatables_Correctly()
        {
            // Arrange
            var leftValue = 13;
            var rightValue = 15;
            var sut = new GreaterThanConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder<int>(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder<int>(rightValue))
                .Build();

            // Act
            var children = sut.GetContainedEvaluatables(true).ToArray();

            // Assert
            children.Length.ShouldBe(2);
        }
    }

    public class ToTypedBuilder : GreaterThanConditionTests
    {
        [Fact]
        public void Returns_Valid_Builder()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "this";
            var sut = new GreaterThanConditionBuilder()
                .WithSourceExpression(new LiteralEvaluatableBuilder<string>(leftValue))
                .WithCompareExpression(new LiteralEvaluatableBuilder<string>(rightValue))
                .BuildTyped();

            // Act
            var typedBuilder = sut.ToTypedBuilder();

            // Assert
            typedBuilder.ShouldBeOfType<GreaterThanConditionBuilder>();
        }
    }
}
