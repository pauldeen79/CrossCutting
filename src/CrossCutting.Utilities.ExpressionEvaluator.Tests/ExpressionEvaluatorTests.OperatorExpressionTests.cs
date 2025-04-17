namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class OperatorExpressionTests : TestBase
{
    protected IOperatorExpressionTokenizer Tokenizer { get; }
    protected IOperatorExpressionParser Parser { get; }
    protected IOperator Operator { get; }

    protected ExpressionEvaluator CreateSut() => new ExpressionEvaluator(Tokenizer, Parser, Enumerable.Empty<IExpression>());

    public OperatorExpressionTests()
    {
        Tokenizer = Substitute.For<IOperatorExpressionTokenizer>();
        Parser = Substitute.For<IOperatorExpressionParser>();
        Operator = Substitute.For<IOperator>();

        Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
    }

    public class Evaluate : OperatorExpressionTests
    {
        //TODO: Review if this path is sensible. I don't think so.
        [Fact]
        public void Returns_Continue_When_Operator_Signs_Are_Not_Present()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<List<OperatorExpressionToken>>([new OperatorExpressionToken(OperatorExpressionTokenType.EOF)]));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Success(Operator));
            Operator.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("some expression without operators"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_NonSuccessful_Result_From_Tokenizer()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Error<List<OperatorExpressionToken>>("Kaboom!"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public void Returns_NonSuccessful_Result_From_Parser()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Error<IOperator>("Kaboom!"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public void Returns_Result_From_Operator_When_Tokenizer_And_Parser_Both_Succeed()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
            Operator.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>(1 + 2));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Success(Operator));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(3);
        }
    }

    public class Parse : OperatorExpressionTests
    {
        //TODO: Review if this path is sensible. I don't think so.
        [Fact]
        public void Returns_Continue_When_Operator_Signs_Are_Not_Present()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<List<OperatorExpressionToken>>([new OperatorExpressionToken(OperatorExpressionTokenType.EOF)]));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Success(Operator));
            Operator.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("some expression without operators"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_NonSuccessful_Result_From_Tokenizer()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Error<List<OperatorExpressionToken>>("Kaboom!"));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public void Returns_NonSuccessful_Result_From_Parser()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Error<IOperator>("Kaboom!"));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public void Returns_Result_From_Operator_When_Tokenizer_And_Parser_Both_Succeed()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
            Operator.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithSourceExpression("1 + 2").WithResultType(typeof(int)));
            Parser.Parse(Arg.Any<ICollection<OperatorExpressionToken>>()).Returns(Result.Success(Operator));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
