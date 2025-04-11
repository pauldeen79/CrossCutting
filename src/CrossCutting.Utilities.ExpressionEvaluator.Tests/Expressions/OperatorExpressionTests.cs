namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class OperatorExpressionTests : TestBase
{
    protected IOperatorExpressionTokenizer Tokenizer { get; }
    protected IOperatorExpressionParser Parser { get; }
    protected IOperator Operator { get; }

    protected OperatorExpression CreateSut() => new OperatorExpression(Tokenizer, Parser);

    public OperatorExpressionTests()
    {
        Tokenizer = Substitute.For<IOperatorExpressionTokenizer>();
        Parser = Substitute.For<IOperatorExpressionParser>();
        Operator = Substitute.For<IOperator>();
    }

    public class Evaluate : OperatorExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Operator_Signs_Are_Not_Present()
        {
            // Arrange
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Error<List<OperatorExpressionToken>>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
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
        [Fact]
        public void Returns_Continue_When_Operator_Signs_Are_Not_Present()
        {
            // Arrange
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Error<List<OperatorExpressionToken>>("Kaboom!"));
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
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
            Tokenizer.Tokenize(Arg.Any<string>()).Returns(Result.Success(new List<OperatorExpressionToken>()));
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
