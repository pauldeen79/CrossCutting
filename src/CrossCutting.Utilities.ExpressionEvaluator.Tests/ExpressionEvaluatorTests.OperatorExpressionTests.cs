namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class OperatorExpressionTests : TestBase
{
    protected IExpressionTokenizer Tokenizer { get; }
    protected IExpressionParser Parser { get; }
    protected IExpression Operator { get; }

    protected ExpressionEvaluator CreateSut() => new ExpressionEvaluator(Tokenizer, Parser, Enumerable.Empty<IExpressionComponent>());

    public OperatorExpressionTests()
    {
        Tokenizer = Substitute.For<IExpressionTokenizer>();
        Parser = Substitute.For<IExpressionParser>();
        Operator = Substitute.For<IExpression>();

        Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
    }

    public class Evaluate : OperatorExpressionTests
    {
        //TODO: Review if this path is sensible. I don't think so.
        [Fact]
        public void Returns_Continue_When_Operator_Signs_Are_Not_Present()
        {
            // Arrange
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<List<ExpressionToken>>([new ExpressionToken(ExpressionTokenType.EOF)]));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Success(Operator));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Error<List<ExpressionToken>>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<ExpressionToken>()));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Error<IExpression>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<ExpressionToken>()));
            Operator.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>(1 + 2));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Success(Operator));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<List<ExpressionToken>>([new ExpressionToken(ExpressionTokenType.EOF)]));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Success(Operator));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Error<List<ExpressionToken>>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<ExpressionToken>()));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Error<IExpression>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(new List<ExpressionToken>()));
            Operator.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithSourceExpression("1 + 2").WithResultType(typeof(int)));
            Parser.Parse(Arg.Any<ICollection<ExpressionToken>>()).Returns(Result.Success(Operator));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
