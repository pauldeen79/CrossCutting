namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionParserTests : TestBase<ExpressionParser>
{
    public class Parse : ExpressionParserTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Logical_Or()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.Or),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false || true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false || true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Logical_And()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.And),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false && true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false && true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Equal()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.Equal),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false == true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false == true);
        }

        [Fact]
        public void Returns_Correct_Result_On_NotEqual()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.NotEqual),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("false != true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false != true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Less()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 < 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 < 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_LessOrEqual()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.LessOrEqual),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 <= 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 <= 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Greater()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 > 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 > 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_GreaterOrEqual()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.GreaterOrEqual),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 >= 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 >= 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Plus()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Plus),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 + 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 + 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Minus()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Minus),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 - 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 - 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Multiply()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Multiply),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 * 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 * 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Divide()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Divide),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 / 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 / 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Modulo()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Modulo),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<BinaryExpression>();
            var binaryOperator = (BinaryExpression)result.Value;
            binaryOperator.Left.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Left.Value.ShouldBeOfType<OtherExpression>();
            binaryOperator.Right.Status.ShouldBe(ResultStatus.Ok);
            binaryOperator.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("1 % 2"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 % 2);
        }

        [Fact]
        public void Returns_Correct_Result_On_Bang()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Bang),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<UnaryExpression>();
            var unaryOperator = (UnaryExpression)result.Value;
            unaryOperator.Operand.Status.ShouldBe(ResultStatus.Ok);
            unaryOperator.Operand.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = result.Value.Evaluate(CreateContext("!true"));
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(!true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Invalid_Token()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_With_Missing_Close_Parenthesis()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Missing_Close_Parenthesis()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "System.String"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Arguments_But_Missing_Close_Parenthesis()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "System.String"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.Other, "arguments"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_Without_Arguments()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_With_Arguments()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.Other, "arguments"),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_Without_Arguments()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "System.String"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Arguments()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "System.String"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.Other, "arguments"),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_Missing_FunctionName_Without_Arguments()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "System.String"),
                new ExpressionToken(ExpressionTokenType.Greater),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Less_Sign_At_Start()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Bang_With_Missing_Expression()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Bang),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Less_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Equal_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Equal),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_And_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.And),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Or_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Or),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Plus_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Plus),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Multiply_With_Error()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Multiply),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Missing_Close_Parenthesis()
        {
            // Arrange
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Plus),
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Expect ')' after expression.");
        }
    }
}
