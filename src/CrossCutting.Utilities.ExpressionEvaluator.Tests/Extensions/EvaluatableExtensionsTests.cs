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

    public class StartsWith_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.StartsWith(new LiteralEvaluatable<string>("T"));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class StartsWith_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.StartsWith("T");

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class EndsWith_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.EndsWith(new LiteralEvaluatable<string>("T"));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class EndsWith_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.EndsWith("T");

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class Contains_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.Contains(new LiteralEvaluatable<string>("T"));

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class Contains_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.Contains("T");

            // Assert
            result.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)result.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            result.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)result.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotStartWith_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotStartWith(new LiteralEvaluatable<string>("T"));

            // Assert
            result.Operand.ShouldBeOfType<StringStartsWithOperatorEvaluatable>();
            var stringStartsWithOperatorEvaluatable = (StringStartsWithOperatorEvaluatable)result.Operand;
            stringStartsWithOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringStartsWithOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringStartsWithOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringStartsWithOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotStartWith_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotStartWith("T");

            // Assert
            result.Operand.ShouldBeOfType<StringStartsWithOperatorEvaluatable>();
            var stringStartsWithOperatorEvaluatable = (StringStartsWithOperatorEvaluatable)result.Operand;
            stringStartsWithOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringStartsWithOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringStartsWithOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringStartsWithOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotEndWith_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotEndWith(new LiteralEvaluatable<string>("T"));

            // Assert
            result.Operand.ShouldBeOfType<StringEndsWithOperatorEvaluatable>();
            var stringEndsWithOperatorEvaluatable = (StringEndsWithOperatorEvaluatable)result.Operand;
            stringEndsWithOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringEndsWithOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringEndsWithOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringEndsWithOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotEndWith_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotEndWith("T");

            // Assert
            result.Operand.ShouldBeOfType<StringEndsWithOperatorEvaluatable>();
            var stringEndsWithOperatorEvaluatable = (StringEndsWithOperatorEvaluatable)result.Operand;
            stringEndsWithOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringEndsWithOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringEndsWithOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringEndsWithOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotContain_Evaluatable : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotContain(new LiteralEvaluatable<string>("T"));

            // Assert
            result.Operand.ShouldBeOfType<StringContainsOperatorEvaluatable>();
            var stringContainsOperatorEvaluatable = (StringContainsOperatorEvaluatable)result.Operand;
            stringContainsOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringContainsOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringContainsOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringContainsOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }

    public class DoesNotContain_Object : EvaluatableExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("Test");

            // Act
            var result = sut.DoesNotContain("T");

            // Assert
            result.Operand.ShouldBeOfType<StringContainsOperatorEvaluatable>();
            var stringContainsOperatorEvaluatable = (StringContainsOperatorEvaluatable)result.Operand;
            stringContainsOperatorEvaluatable.LeftOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var leftOperand = (LiteralEvaluatable<string>)stringContainsOperatorEvaluatable.LeftOperand;
            leftOperand.Value.ShouldBe("Test");
            stringContainsOperatorEvaluatable.RightOperand.ShouldBeOfType<LiteralEvaluatable<string>>();
            var rightOperand = (LiteralEvaluatable<string>)stringContainsOperatorEvaluatable.RightOperand;
            rightOperand.Value.ShouldBe("T");
        }
    }
}