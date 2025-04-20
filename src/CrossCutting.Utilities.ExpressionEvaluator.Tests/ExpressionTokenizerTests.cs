namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionTokenizerTests : TestBase<ExpressionTokenizer>
{
    public ExpressionTokenizerTests()
    {
        Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
    }

    public class Tokenize : ExpressionTokenizerTests
    {
        [Fact]
        public void Returns_Correct_Result_For_Value_With_Quotes()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("\"hello\" == \"world\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("\"hello\"");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Equal);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[2].Value.ShouldBe("\"world\"");
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_End_Quote()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("\"hello\" == \"world"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("\"hello\"");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Equal);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[2].Value.ShouldBe("\"world");
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Theory]
        [InlineData("+", ExpressionTokenType.Plus)]
        [InlineData("-", ExpressionTokenType.Minus)]
        [InlineData("*", ExpressionTokenType.Multiply)]
        [InlineData("/", ExpressionTokenType.Divide)]
        [InlineData("%", ExpressionTokenType.Modulo)]
        [InlineData("^", ExpressionTokenType.Exponentiation)]
        [InlineData("<", ExpressionTokenType.Less)]
        [InlineData("<=", ExpressionTokenType.LessOrEqual)]
        [InlineData(">", ExpressionTokenType.Greater)]
        [InlineData(">=", ExpressionTokenType.GreaterOrEqual)]
        [InlineData("==", ExpressionTokenType.Equal)]
        [InlineData("!=", ExpressionTokenType.NotEqual)]
        [InlineData("&&", ExpressionTokenType.And)]
        [InlineData("||", ExpressionTokenType.Or)]
        public void Returns_Correct_Result_For_Value_With_Operator(string sign, ExpressionTokenType expectedType)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext($"1 {sign} 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(expectedType);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[2].Value.ShouldBe("2");
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Forced_Negative_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("-1 + 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("-1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[2].Value.ShouldBe("2");
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Forced_Positive_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 + +2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[2].Value.ShouldBe("+2");
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_Number_After_Plus_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 + +"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Plus);
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_Number_After_Minus_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 - -"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Minus);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.Minus);
            result.Value[3].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Invalid_Character_After_Equal_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 = 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unexpected '='");
        }

        [Fact]
        public void Returns_Correct_Result_For_Invalid_Character_After_Ampersand_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 & 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Single '&' is not supported.");
        }

        [Fact]
        public void Returns_Correct_Result_For_Invalid_Character_After_Pipe_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 | 2"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Single '|' is not supported.");
        }

        [Fact]
        public void Returns_Correct_Result_For_Plus_Operator_At_End_Of_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 +"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(3);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_And_Operator_At_End_Of_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 &&"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(3);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.And);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_SmallerThan_Operator_At_End_Of_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize(CreateContext("1 <"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(3);
            result.Value[0].Type.ShouldBe(ExpressionTokenType.Other);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(ExpressionTokenType.Less);
            result.Value[2].Type.ShouldBe(ExpressionTokenType.EOF);
        }
    }
}
