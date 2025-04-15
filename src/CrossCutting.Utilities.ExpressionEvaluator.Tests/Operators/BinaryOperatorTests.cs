namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Operators;

public class BinaryOperatorTests : TestBase
{
    protected IOperator Operator { get; }

    public BinaryOperatorTests()
    {
        Operator = Substitute.For<IOperator>();
    }

    public class Evaluate : BinaryOperatorTests
    {
        [Fact]
        public void Returns_Error_From_Left_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var sut = new BinaryOperator(Result.Error<IOperator>("Kaboom"), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Right_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Error<IOperator>("Kaboom"), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Left_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Error<object?>("Kaboom"));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Right_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                 {
                     counter++;
                     if (counter == 2)
                     {
                         return Result.Error<object?>("Kaboom");
                     }

                     return Result.Success<object?>(counter);
                 });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Invalid_On_Unsupported_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.LeftParenthesis, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unsupported operator: LeftParenthesis");
        }

        [Fact]
        public void Returns_Success_On_Plus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 + 2);
        }

        [Fact]
        public void Returns_Success_On_Minus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Minus, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 - 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 - 2);
        }

        [Fact]
        public void Returns_Success_On_Multiply_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Multiply, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 * 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 * 2);
        }

        [Fact]
        public void Returns_Success_On_Divide_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Divide, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 / 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 / 2);
        }

        [Fact]
        public void Returns_Success_On_Equal_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Equal, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 == 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 == 2);
        }

        [Fact]
        public void Returns_Success_On_NotEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.NotEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 != 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 != 2);
        }

        [Fact]
        public void Returns_Success_On_Less_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Less, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 < 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 < 2);
        }

        [Fact]
        public void Returns_Success_On_LessOrEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.LessOrEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 <= 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 <= 2);
        }

        [Fact]
        public void Returns_Success_On_Greater_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Greater, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 > 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 > 2);
        }

        [Fact]
        public void Returns_Success_On_GreaterOrEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.GreaterOrEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 >= 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 >= 2);
        }

        [Fact]
        public void Returns_Success_On_And_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.And, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 && 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.IsTruthy() && 2.IsTruthy());
        }

        [Fact]
        public void Returns_Success_On_Or_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Or, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 || 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.IsTruthy() || 2.IsTruthy());
        }

        [Fact]
        public void Returns_Success_On_Modulus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Modulo, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 % 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 % 2);
        }

        [Fact]
        public void Returns_Success_On_Exponential_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Exponentiation, Result.Success(Operator), expression);

            // Act
            var result = sut.Evaluate(CreateContext("1 ^ 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1);
        }
    }

    public class Parse : BinaryOperatorTests
    {
        [Fact]
        public void Returns_Error_From_Left_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var sut = new BinaryOperator(Result.Error<IOperator>("Kaboom"), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Right_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Error<IOperator>("Kaboom"), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Left_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Right_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var counter = 0;
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(_ =>
                {
                    counter++;
                    if (counter == 2)
                    {
                        return new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error);
                    }

                    return new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok);
                });

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Invalid_On_Unsupported_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.LeftParenthesis, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unsupported operator: LeftParenthesis");
        }

        [Fact]
        public void Returns_Success_On_Plus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Minus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Minus, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 - 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Multiply_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Multiply, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 * 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Divide_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Divide, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 / 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Equal_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Equal, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 == 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_NotEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.NotEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 != 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Less_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Less, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 < 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_LessOrEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.LessOrEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 <= 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Greater_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Greater, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 > 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_GreaterOrEqual_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.GreaterOrEqual, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 >= 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_And_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.And, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 && 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_On_Or_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            Operator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryOperator(Result.Success(Operator), OperatorExpressionTokenType.Or, Result.Success(Operator), expression);

            // Act
            var result = sut.Parse(CreateContext("1 || 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }
    }
}
