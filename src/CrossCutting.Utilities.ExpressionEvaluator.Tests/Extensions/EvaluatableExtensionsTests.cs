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
                .WithLeftOperand(new LiteralEvaluatableBuilder(true))
                .WithRightOperand(new LiteralEvaluatableBuilder(true))
                .Build();

            // Act
            var result = sut.And(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(false))
                .WithRightOperand(new LiteralEvaluatableBuilder(false))
                .Build());

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var leftOperand = (EqualOperatorEvaluatable)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var rightOperand = (EqualOperatorEvaluatable)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }

    public class Or : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(true))
                .WithRightOperand(new LiteralEvaluatableBuilder(true))
                .Build();

            // Act
            var result = sut.Or(new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(false))
                .WithRightOperand(new LiteralEvaluatableBuilder(false))
                .Build());

            // Assert
            result.LeftOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var leftOperand = (EqualOperatorEvaluatable)result.LeftOperand;
            leftOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)leftOperand.LeftOperand).Value.ShouldBe(true);
            leftOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)leftOperand.RightOperand).Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<EqualOperatorEvaluatable>();
            var rightOperand = (EqualOperatorEvaluatable)result.RightOperand;
            rightOperand.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)rightOperand.LeftOperand).Value.ShouldBe(false);
            rightOperand.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            ((LiteralEvaluatable)rightOperand.RightOperand).Value.ShouldBe(false);
        }
    }

    public class IsEqualTo_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsEqualTo(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsEqualTo_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsEqualTo(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterOrEqualThan_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsGreaterOrEqualThan(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterOrEqualThan_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsGreaterOrEqualThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterThan_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsGreaterThan(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterThan_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsGreaterThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerOrEqualThan_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsSmallerOrEqualThan(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerOrEqualThan_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsSmallerOrEqualThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerThan_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsSmallerThan(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerThan_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsSmallerThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotEqualTo_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsNotEqualTo(new LiteralEvaluatable(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotEqualTo_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsNotEqualTo(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable>();
            var leftOperand = (LiteralEvaluatable)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable>();
            var rightOperand = (LiteralEvaluatable)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotNull : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsNotNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatable>();
            var operand = (LiteralEvaluatable)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }

    public class IsNull : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable(true);

            // Act
            var result = sut.IsNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatable>();
            var operand = (LiteralEvaluatable)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }
}