namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class OperatorExpressionParserTests : TestBase<OperatorExpressionParser>
{
    public class Parse : OperatorExpressionParserTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Logical_Or()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "false"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Or),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "true"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false || true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false || true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Logical_And()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "false"),
                new OperatorExpressionToken(OperatorExpressionTokenType.And),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "true"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false && true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false && true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Equal()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "false"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Equal),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "true"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false == true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false == true);
        }

        [Fact]
        public void Returns_Correct_Result_On_NotEqual()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "false"),
                new OperatorExpressionToken(OperatorExpressionTokenType.NotEqual),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "true"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false != true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false != true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Less()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Less),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 < 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 < 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_LessOrEqual()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.LessOrEqual),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 <= 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 <= 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Greater()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Greater),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 > 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 > 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_GreaterOrEqual()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.GreaterOrEqual),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 >= 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 >= 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Plus()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Plus),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 + 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 + 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Minus()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Minus),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 - 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 - 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Multiply()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Multiply),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 * 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 * 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Divide()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Divide),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 / 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 / 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Modulo()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "1"),
                new OperatorExpressionToken(OperatorExpressionTokenType.Modulo),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "2"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryOperator>();
            var binaryOperator = (BinaryOperator)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<ExpressionOperator>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 % 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 % 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Bang()
        {
            // Arrange
            var tokens = new List<OperatorExpressionToken>
            {
                new OperatorExpressionToken(OperatorExpressionTokenType.Bang),
                new OperatorExpressionToken(OperatorExpressionTokenType.Expression, "true"),
                new OperatorExpressionToken(OperatorExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<UnaryOperator>();
            var unaryOperator = (UnaryOperator)result.Value;
            unaryOperator.Operand.Status.ShouldBe(ResultStatus.Ok);
            unaryOperator.Operand.Value.ShouldBeOfType<ExpressionOperator>();
            var evaluationResult = result.Value.Evaluate(CreateContext("!true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(!true);
        }
    }
}
