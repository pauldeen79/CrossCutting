namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotEndsWithConditionTests : TestBase<StringNotEndsWithCondition>
{
    public class Evaluate : StringNotEndsWithConditionTests
    {
        [Fact]
        public async Task Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "t";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
                { nameof(IDoubleExpressionContainer.SecondExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(rightValue).Build() },
                { nameof(IStringComparisonContainer.StringComparison).ToCamelCase(CultureInfo.CurrentCulture), StringComparison },
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
        public async Task Returns_Invalid_On_Left_No_String()
        {
            // Arrange
            var leftValue = 1;
            var rightValue = "this";
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
                { nameof(IDoubleExpressionContainer.SecondExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(rightValue).Build() },
                { nameof(IStringComparisonContainer.StringComparison).ToCamelCase(CultureInfo.CurrentCulture), StringComparison },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }

        [Fact]
        public async Task Returns_Invalid_On_Right_No_String()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var parameters = new Dictionary<string, object?>
            {
                { nameof(IDoubleExpressionContainer.FirstExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(leftValue).Build() },
                { nameof(IDoubleExpressionContainer.SecondExpression).ToCamelCase(CultureInfo.CurrentCulture), new LiteralExpressionBuilder(rightValue).Build() },
                { nameof(IStringComparisonContainer.StringComparison).ToCamelCase(CultureInfo.CurrentCulture), StringComparison },
            };
            var sut = CreateSut(parameters);
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }
    }
}
