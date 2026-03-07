namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Builders.Extensions;

public class EvaluatableBuilderExtensionsTests : TestBase
{
    public class And : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(true));

            // Act
            var result = sut.And(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(false))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false)));

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
            var leftOperand = (EqualOperatorEvaluatableBuilder)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
            var rightOperand = (EqualOperatorEvaluatableBuilder)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }

    public class Or : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(true));

            // Act
            var result = sut.Or(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(false))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false)));

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
            var leftOperand = (EqualOperatorEvaluatableBuilder)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
            var rightOperand = (EqualOperatorEvaluatableBuilder)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            ((LiteralEvaluatableBuilder<bool>)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }
}