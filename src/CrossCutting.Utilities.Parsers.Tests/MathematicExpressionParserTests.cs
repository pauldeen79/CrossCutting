namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class MathematicExpressionParserTests : IDisposable
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;

    [Fact]
    public void Can_Add_One_And_One()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(Math.Pow(2, 4));
    }

    [Fact]
    public void Can_Use_Modulus()
    {
        // Arrange
        var input = "5 % 2";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5 % 2);
    }

    [Fact]
    public void Can_Add_One_And_Two_And_Three()
    {
        // Arrange
        var input = "1 + 2 + 3";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Too many closing braces found");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "(1 + 2";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing 1 close brace");
    }

    [Fact]
    public void Missing_CloseBrackets_Returns_NotFound()
    {
        // Arrange
        var input = "((1 + 2";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing 2 close braces");
    }

    [Fact]
    public void Expression_Starting_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "+ 2";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt64).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateDecimal).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateSingle).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateDouble).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateByte).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateInt16).Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut(ParseExpressionDelegateDouble).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(11);
    }

    [Fact]
    public void Returns_Error_From_Left_Expression()
    {
        // Arrange
        var input = "bicycle + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Right_Expression()
    {
        // Arrange
        var input = "1 + bicycle";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Nested_Expression()
    {
        // Arrange
        var input = "(1 + bicycle)";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Aggregation_Operation()
    {
        // Arrange
        var input = "1 / 0";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt32).Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Aggregation failed. Error message: Attempted to divide by zero.");
    }

    private IMathematicExpressionParser CreateSut(Func<string, IFormatProvider, Result<object?>> dlg)
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IExpressionParser>(new MyMathematicExpressionParser(dlg))
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<IMathematicExpressionParser>();
    }

    private Result<object?> ParseExpressionDelegateInt32(string arg, IFormatProvider formatProvider)
        => int.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to integer");

    private Result<object?> ParseExpressionDelegateInt64(string arg, IFormatProvider formatProvider)
        => long.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to long");

    private Result<object?> ParseExpressionDelegateDouble(string arg, IFormatProvider formatProvider)
        => double.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to double");

    private Result<object?> ParseExpressionDelegateSingle(string arg, IFormatProvider formatProvider)
        => float.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to float");

    private Result<object?> ParseExpressionDelegateDecimal(string arg, IFormatProvider formatProvider)
        => decimal.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to decimal");

    private Result<object?> ParseExpressionDelegateByte(string arg, IFormatProvider formatProvider)
        => byte.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to byte");

    private Result<object?> ParseExpressionDelegateInt16(string arg, IFormatProvider formatProvider)
        => short.TryParse(arg, formatProvider, out var result)
            ? Result<object?>.Success(result)
            : Result<object?>.Invalid($"Could not parse {arg} to short");

    public void Dispose()
    {
        _scope?.Dispose();
        _provider?.Dispose();
    }

    private sealed class MyMathematicExpressionParser : IExpressionParser
    {
        private readonly Func<string, IFormatProvider, Result<object?>> _dlg;

        public MyMathematicExpressionParser(Func<string, IFormatProvider, Result<object?>> dlg)
        {
            _dlg = dlg;
        }

        public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
            => _dlg.Invoke(value, formatProvider);
    }
}
