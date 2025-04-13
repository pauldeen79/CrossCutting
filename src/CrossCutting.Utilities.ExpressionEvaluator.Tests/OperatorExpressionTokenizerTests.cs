namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class OperatorExpressionTokenizerTests : TestBase<OperatorExpressionTokenizer>
{
    public class Tokenize : OperatorExpressionTokenizerTests
    {
        [Fact]
        public void Returns_Correct_Result_For_Value_With_Quotes()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize("\"hello\" == \"world\"");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(2);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("\"hello\" == \"world\"");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_End_Quote()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize("\"hello\" == \"world");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(2);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("\"hello\" == \"world");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Theory]
        [InlineData("+", OperatorExpressionTokenType.Plus)]
        [InlineData("-", OperatorExpressionTokenType.Minus)]
        [InlineData("*", OperatorExpressionTokenType.Multiply)]
        [InlineData("/", OperatorExpressionTokenType.Divide)]
        [InlineData("%", OperatorExpressionTokenType.Modulo)]
        [InlineData("^", OperatorExpressionTokenType.Exponentiation)]
        [InlineData("<", OperatorExpressionTokenType.Less)]
        [InlineData("<=", OperatorExpressionTokenType.LessOrEqual)]
        [InlineData(">", OperatorExpressionTokenType.Greater)]
        [InlineData(">=", OperatorExpressionTokenType.GreaterOrEqual)]
        [InlineData("==", OperatorExpressionTokenType.Equal)]
        [InlineData("!=", OperatorExpressionTokenType.NotEqual)]
        [InlineData("&&", OperatorExpressionTokenType.And)]
        [InlineData("||", OperatorExpressionTokenType.Or)]
        public void Returns_Correct_Result_For_Value_With_Operator(string sign, OperatorExpressionTokenType expectedType)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize($"1 {sign} 2");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(expectedType);
            result.Value[2].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[2].Value.ShouldBe("2");
            result.Value[3].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Forced_Negative_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize($"-1 + 2");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("-1");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[2].Value.ShouldBe("2");
            result.Value[3].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Forced_Positive_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize($"1 + +2");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[2].Value.ShouldBe("+2");
            result.Value[3].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_Number_After_Plus_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize($"1 + +");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.Plus);
            result.Value[2].Type.ShouldBe(OperatorExpressionTokenType.Plus);
            result.Value[3].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }

        [Fact]
        public void Returns_Correct_Result_For_Value_With_Missing_Number_After_Minus_Sign()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Tokenize($"1 - -");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(4);
            result.Value[0].Type.ShouldBe(OperatorExpressionTokenType.Expression);
            result.Value[0].Value.ShouldBe("1");
            result.Value[1].Type.ShouldBe(OperatorExpressionTokenType.Minus);
            result.Value[2].Type.ShouldBe(OperatorExpressionTokenType.Minus);
            result.Value[3].Type.ShouldBe(OperatorExpressionTokenType.EOF);
        }
    }
}
