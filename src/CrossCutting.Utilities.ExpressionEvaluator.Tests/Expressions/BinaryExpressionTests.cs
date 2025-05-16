namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class BinaryExpressionTests : TestBase
{
    protected IExpression Operator { get; }

    public BinaryExpressionTests()
    {
        Operator = Substitute.For<IExpression>();
        Operator
            .EvaluateAsync()
            .Returns(Result.NoContent<object?>());
    }

    public class Evaluate : BinaryExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Left_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var sut = new BinaryExpression(context, Result.Error<IExpression>("Kaboom"), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Right_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Error<IExpression>("Kaboom"), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Left_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            Operator
                .EvaluateAsync()
                .Returns(Result.Error<object?>("Kaboom"));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Right_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                 {
                     counter++;
                     if (counter == 2)
                     {
                         return Result.Error<object?>("Kaboom");
                     }

                     return Result.Success<object?>(counter);
                 });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_On_Unsupported_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.LeftParenthesis, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unsupported operator: LeftParenthesis");
        }

        [Fact]
        public async Task Returns_Success_On_Plus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 + 2);
        }

        [Fact]
        public async Task Returns_Success_On_Minus_Operator()
        {
            // Arrange
            var expression = "1 - 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Minus, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 - 2);
        }

        [Fact]
        public async Task Returns_Success_On_Multiply_Operator()
        {
            // Arrange
            var expression = "1 * 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Multiply, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 * 2);
        }

        [Fact]
        public async Task Returns_Success_On_Divide_Operator()
        {
            // Arrange
            var expression = "1 / 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Divide, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 / 2);
        }

        [Fact]
        public async Task Returns_Success_On_Equal_Operator()
        {
            // Arrange
            var expression = "1 == 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Equal, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 == 2);
        }

        [Fact]
        public async Task Returns_Success_On_NotEqual_Operator()
        {
            // Arrange
            var expression = "1 != 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.NotEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 != 2);
        }

        [Fact]
        public async Task Returns_Success_On_Less_Operator()
        {
            // Arrange
            var expression = "1 < 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Less, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 < 2);
        }

        [Fact]
        public async Task Returns_Success_On_LessOrEqual_Operator()
        {
            // Arrange
            var expression = "1 <= 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.LessOrEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 <= 2);
        }

        [Fact]
        public async Task Returns_Success_On_Greater_Operator()
        {
            // Arrange
            var expression = "1 > 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Greater, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 > 2);
        }

        [Fact]
        public async Task Returns_Success_On_GreaterOrEqual_Operator()
        {
            // Arrange
            var expression = "1 >= 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.GreaterOrEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 >= 2);
        }

        [Fact]
        public async Task Returns_Success_On_And_Operator()
        {
            // Arrange
            var expression = "1 && 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.And, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.IsTruthy() && 2.IsTruthy());
        }

        [Fact]
        public async Task Returns_Success_On_Or_Operator()
        {
            // Arrange
            var expression = "1 || 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Or, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1.IsTruthy() || 2.IsTruthy());
        }

        [Fact]
        public async Task Returns_Success_On_Modulus_Operator()
        {
            // Arrange
            var expression = "1 % 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Modulo, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 % 2);
        }

        [Fact]
        public async Task Returns_Success_On_Exponential_Operator()
        {
            // Arrange
            var expression = "1 ^ 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .EvaluateAsync()
                .Returns(_ =>
                {
                    counter++;

                    return Result.Success<object?>(counter);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Exponentiation, Result.Success(Operator), expression);

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1);
        }
    }

    public class Parse : BinaryExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Left_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var sut = new BinaryExpression(context, Result.Error<IExpression>("Kaboom"), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Right_Expression()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Error<IExpression>("Kaboom"), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Left_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Right_Expression_Parsing()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            var counter = 0;
            Operator
                .ParseAsync()
                .Returns(_ =>
                {
                    counter++;
                    if (counter == 2)
                    {
                        return new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error);
                    }

                    return new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok);
                });

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_On_Unsupported_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.LeftParenthesis, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unsupported operator: LeftParenthesis");
        }

        [Fact]
        public async Task Returns_Success_On_Plus_Operator()
        {
            // Arrange
            var expression = "1 + 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Plus, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Minus_Operator()
        {
            // Arrange
            var expression = "1 - 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Minus, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Multiply_Operator()
        {
            // Arrange
            var expression = "1 * 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Multiply, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Divide_Operator()
        {
            // Arrange
            var expression = "1 / 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Divide, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Equal_Operator()
        {
            // Arrange
            var expression = "1 == 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Equal, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_NotEqual_Operator()
        {
            // Arrange
            var expression = "1 != 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.NotEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Less_Operator()
        {
            // Arrange
            var expression = "1 < 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Less, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_LessOrEqual_Operator()
        {
            // Arrange
            var expression = "1 <= 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.LessOrEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Greater_Operator()
        {
            // Arrange
            var expression = "1 > 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Greater, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_GreaterOrEqual_Operator()
        {
            // Arrange
            var expression = "1 >= 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.GreaterOrEqual, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_And_Operator()
        {
            // Arrange
            var expression = "1 && 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.And, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Returns_Success_On_Or_Operator()
        {
            // Arrange
            var expression = "1 || 2";
            var context = CreateContext(expression);
            Operator
                .ParseAsync()
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Ok).WithResultType(typeof(int)));

            var sut = new BinaryExpression(context, Result.Success(Operator), ExpressionTokenType.Or, Result.Success(Operator), expression);

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
            result.PartResults.Count.ShouldBe(2);
        }
    }
}
