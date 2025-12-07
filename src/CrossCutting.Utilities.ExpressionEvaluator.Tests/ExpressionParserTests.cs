namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionParserTests : TestBase<ExpressionParser>
{
    public class ParseAsync : ExpressionParserTests
    {
        [Fact]
        public async Task Returns_Correct_Result_On_Logical_Or()
        {
            // Arrange
            var context = CreateContext("false || true");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.Or),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false || true);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Logical_And()
        {
            // Arrange
            var context = CreateContext("false && true");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.And),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false && true);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Equal()
        {
            // Arrange
            var context = CreateContext("false == true");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.Equal),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<EqualOperatorExpression>();
            var operatorExpression = (EqualOperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false == true);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_NotEqual()
        {
            // Arrange
            var context = CreateContext("false != true");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "false"),
                new ExpressionToken(ExpressionTokenType.NotEqual),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<NotEqualOperatorExpression>();
            var operatorExpression = (NotEqualOperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(false != true);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Plus()
        {
            // Arrange
            var context = CreateContext("1 + 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Plus),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 + 2);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Minus()
        {
            // Arrange
            var context = CreateContext("1 - 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Minus),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 - 2);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Multiply()
        {
            // Arrange
            var context = CreateContext("1 * 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Multiply),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 * 2);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Divide()
        {
            // Arrange
            var context = CreateContext("1 / 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Divide),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 / 2);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Modulo()
        {
            // Arrange
            var context = CreateContext("1 % 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Modulus),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OperatorExpression>();
            var operatorExpression = (OperatorExpression)result.Value;
            operatorExpression.Left.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Left.Value.ShouldBeOfType<OtherExpression>();
            operatorExpression.Right.Status.ShouldBe(ResultStatus.Ok);
            operatorExpression.Right.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(1 % 2);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Bang()
        {
            // Arrange
            var context = CreateContext("!true");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Bang),
                new ExpressionToken(ExpressionTokenType.Other, "true"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<UnaryExpression>();
            var unaryOperator = (UnaryExpression)result.Value;
            unaryOperator.Operand.Status.ShouldBe(ResultStatus.Ok);
            unaryOperator.Operand.Value.ShouldBeOfType<OtherExpression>();
            var evaluationResult = await result.Value.EvaluateAsync(context, CancellationToken.None);
            evaluationResult.Status.ShouldBe(ResultStatus.Ok);
            evaluationResult.Value.ShouldBe(!true);
        }

        [Fact]
        public void Returns_Correct_Result_On_Invalid_Token()
        {
            // Arrange
            var context = CreateContext("(");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_With_Missing_Close_Parenthesis()
        {
            // Arrange
            var context = CreateContext("MyFunction(");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Missing_Close_Parenthesis()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Arguments_But_Missing_Close_Parenthesis()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(arguments");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Missing right parenthesis");
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis),
                new ExpressionToken(ExpressionTokenType.RightParenthesis),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Function_With_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction(arguments)");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Nested_Function()
        {
            // Arrange
            var context = CreateContext("MyFunction(arguments)");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "MyFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis, "("),
                new ExpressionToken(ExpressionTokenType.Other, "MyNestedFunction"),
                new ExpressionToken(ExpressionTokenType.LeftParenthesis, "("),
                new ExpressionToken(ExpressionTokenType.Other, "arguments"),
                new ExpressionToken(ExpressionTokenType.RightParenthesis, ")"),
                new ExpressionToken(ExpressionTokenType.RightParenthesis, ")"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)result.Value).Expression.ShouldBe("MyFunction(MyNestedFunction(arguments))");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_With_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(arguments)");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_Function_Missing_FunctionName_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("<System.String>()");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Less_Sign_At_Start()
        {
            // Arrange
            var context = CreateContext("< 2");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.Other, "2"),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Bang_With_Missing_Expression()
        {
            // Arrange
            var context = CreateContext("!");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Bang),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Less_With_Error()
        {
            // Arrange
            var context = CreateContext("1 < ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Less),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Equal_With_Error()
        {
            // Arrange
            var context = CreateContext("1 == ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Equal),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_And_With_Error()
        {
            // Arrange
            var context = CreateContext("1 && ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.And),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Or_With_Error()
        {
            // Arrange
            var context = CreateContext("1 || ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Or),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Plus_With_Error()
        {
            // Arrange
            var context = CreateContext("1 + ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Plus),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Multiply_With_Error()
        {
            // Arrange
            var context = CreateContext("1 * ");
            var tokens = new List<ExpressionToken>
            {
                new ExpressionToken(ExpressionTokenType.Other, "1"),
                new ExpressionToken(ExpressionTokenType.Multiply),
                new ExpressionToken(ExpressionTokenType.EOF),
            };
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected token");
        }

        [Fact]
        public void Returns_Correct_Result_On_Missing_Close_Parenthesis()
        {
            // Arrange
            var context = CreateContext("(1 + 1");
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
            var result = sut.Parse(context, tokens);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Expect ')' after expression.");
        }
    }
}
