namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class EvaluatableExtensionsTests : TestBase
{
    public class And : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(true))
                .Build();

            // Act
            var result = sut.And(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(false))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false))
                .Build());

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var leftOperand = (EqualOperatorEvaluatable)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var rightOperand = (EqualOperatorEvaluatable)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }

    public class Or : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(true))
                .Build();

            // Act
            var result = sut.Or(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(false))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false))
                .Build());

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var leftOperand = (EqualOperatorEvaluatable)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var rightOperand = (EqualOperatorEvaluatable)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            ((LiteralEvaluatable<bool>)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }
}