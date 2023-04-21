namespace CrossCutting.Utilities.Parsers.Tests;

public class MathematicExpressionParserTests
{
    [Fact]
    public void Can_Add_One_And_One()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Subtract_One_And_One()
    {
        // Arrange
        var input = "1 - 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 - 1);
    }

    [Fact]
    public void Can_Multiply_Two_And_Three()
    {
        // Arrange
        var input = "2 * 3";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2 * 3);
    }

    [Fact]
    public void Can_Divide_Six_By_Two()
    {
        // Arrange
        var input = "6 / 2";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(6 / 2);
    }

    [Fact]
    public void Can_Use_Power()
    {
        // Arrange
        var input = "2 ^ 4";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2 ^ 4);
    }

    [Fact]
    public void Can_Add_One_And_Two_And_Three()
    {
        // Arrange
        var input = "1 + 2 + 3";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 2 + 3);
    }

    [Fact]
    public void Can_Use_Nested_Formula()
    {
        // Arrange
        var input = "(1 + 2) * 3";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo((1 + 2) * 3);
    }

    [Fact]
    public void Can_Use_Correct_Operator_Priority()
    {
        // Arrange
        var input = "1 + 2 * 3";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 2 * 3);
    }

    [Fact]
    public void Can_Use_Numeric_Literal()
    {
        // Arrange
        var input = "1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1);
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "1 + 2)";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Too many closing brackets found");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "(1 + 2";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing 1 close bracket");
    }

    [Fact]
    public void Expression_Starting_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "+ 2";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot start with an operator");
    }

    [Fact]
    public void Expression_Ending_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "1 +";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot end with an operator");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Returns_NotFound()
    {
        // Arrange
        var input = "1 ++ 2";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Separated_By_Space_Returns_NotFound()
    {
        // Arrange
        var input = "1 + + 2";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Empty_String_Returns_NotFound()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot be null or empty");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_NotFound()
    {
        // Arrange
        var input = "This string contains the magic `` internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot contain ``, as this is used internally for formatting");
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Int64()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt64);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Decimal()
    {
        // Arrange
        var input = "3.5 + 3.6";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateDecimal);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(7.1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Single()
    {
        // Arrange
        var input = "1.4 + 1.3";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateSingle);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<float>();
        ((float)result.Value!).Should().BeApproximately(1.4f + 1.3f, 0.1f);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Double()
    {
        // Arrange
        var input = "1.7 + 1.4";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateDouble);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1.7 + 1.4);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Byte()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateByte);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Short()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt16);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Handles_Order_Correctly()
    {
        // Arrange
        var input = "7-16/8*2+8";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateDouble);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(11);
    }

    [Fact]
    public void Returns_Error_From_Left_Expression()
    {
        // Arrange
        var input = "fiets + 1";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse fiets to integer");
    }

    [Fact]
    public void Returns_Error_From_Right_Expression()
    {
        // Arrange
        var input = "1 + fiets";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse fiets to integer");
    }

    [Fact]
    public void Returns_Error_From_Nested_Expression()
    {
        // Arrange
        var input = "(1 + fiets)";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse fiets to integer");
    }

    [Fact]
    public void Returns_Error_From_Aggregation_Operation()
    {
        // Arrange
        var input = "1 / 0";

        // Act
        var result = MathematicExpressionParser.Parse(input, ParseExpressionDelegateInt32);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Aggregation failed. Error message: Attempted to divide by zero.");
    }

    private Result<object> ParseExpressionDelegateInt32(string arg)
        => int.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to integer");

    private Result<object> ParseExpressionDelegateInt64(string arg)
        => long.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to long");

    private Result<object> ParseExpressionDelegateDouble(string arg)
        => double.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to double");

    private Result<object> ParseExpressionDelegateSingle(string arg)
        => float.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to float");

    private Result<object> ParseExpressionDelegateDecimal(string arg)
        => decimal.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to decimal");

    private Result<object> ParseExpressionDelegateByte(string arg)
        => byte.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to byte");

    private Result<object> ParseExpressionDelegateInt16(string arg)
        => short.TryParse(arg, CultureInfo.InvariantCulture, out var result)
            ? Result<object>.Success(result)
            : Result<object>.Invalid($"Could not parse {arg} to short");
}
