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

    public class IsEqualTo : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsEqualTo(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterOrEqualThan : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsGreaterOrEqualThan(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterThan : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsGreaterThan(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerOrEqualThan : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsSmallerOrEqualThan(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerThan : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsSmallerThan(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotEqualTo : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsNotEqualTo(new LiteralEvaluatable<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var leftOperand = (LiteralEvaluatable<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var rightOperand = (LiteralEvaluatable<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotNull : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsNotNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var operand = (LiteralEvaluatable<bool>)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }

    public class IsNull : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true).Build();

            // Act
            var result = sut.IsNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatable<bool>>();
            var operand = (LiteralEvaluatable<bool>)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }
}