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

    public class IsEqualTo_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsEqualTo(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsEqualTo_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsEqualTo(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterOrEqualThan_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsGreaterOrEqualThan(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterOrEqualThan_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsGreaterOrEqualThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterThan_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsGreaterThan(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsGreaterThan_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsGreaterThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }
    
    public class IsSmallerOrEqualThan_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsSmallerOrEqualThan(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerOrEqualThan_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsSmallerOrEqualThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerThan_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsSmallerThan(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsSmallerThan_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsSmallerThan(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotEqualTo_EvaluatableBuilder : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsNotEqualTo(new LiteralEvaluatableBuilder<bool>(false));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var rightOperand = (LiteralEvaluatableBuilder<bool>)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNotEqualTo_Object : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsNotEqualTo(false);

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var leftOperand = (LiteralEvaluatableBuilder<bool>)result.LeftOperand;
            leftOperand.Value.ShouldBe(true);
            result.RightOperand.ShouldBeOfType<LiteralEvaluatableBuilder>();
            var rightOperand = (LiteralEvaluatableBuilder)result.RightOperand;
            rightOperand.Value.ShouldBe(false);
        }
    }

    public class IsNull : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var operand = (LiteralEvaluatableBuilder<bool>)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }

    public class IsNotNull : EvaluatableBuilderExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatableBuilder<bool>(true);

            // Act
            var result = sut.IsNotNull();

            // Assert
            result.Operand.ShouldBeOfType<LiteralEvaluatableBuilder<bool>>();
            var operand = (LiteralEvaluatableBuilder<bool>)result.Operand;
            operand.Value.ShouldBe(true);
        }
    }
}