namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class MathematicExpressionEvaluatorTests : IDisposable
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;
    private readonly IVariable _variable = Substitute.For<IVariable>();

    [Fact]
    public void Can_Add_One_And_One()
    {
        // Arrange
        var input = "1 + 1";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Variable()
    {
        // Arrange
        var input = "1 + $myvariable";
        _variable!.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "myvariable" ? Result.Success<object?>(1) : Result.Continue<object?>());
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Subtract_One_And_One()
    {
        // Arrange
        var input = "1 - 1";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 - 1);
    }

    [Fact]
    public void Can_Multiply_Two_And_Three()
    {
        // Arrange
        var input = "2 * 3";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2 * 3);
    }

    [Fact]
    public void Can_Divide_Six_By_Two()
    {
        // Arrange
        var input = "6 / 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(6 / 2);
    }

    [Fact]
    public void Can_Use_Power()
    {
        // Arrange
        var input = "2 ^ 4";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((int)Math.Pow(2, 4));
    }

    [Fact]
    public void Can_Use_Modulus()
    {
        // Arrange
        var input = "5 % 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(5 % 2);
    }

    [Fact]
    public void Can_Add_One_And_Two_And_Three()
    {
        // Arrange
        var input = "1 + 2 + 3";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 2 + 3);
    }

    [Fact]
    public void Can_Use_Nested_Formula()
    {
        // Arrange
        var input = "(1 + 2) * 3";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((1 + 2) * 3);
    }

    [Fact]
    public void Can_Use_Correct_Operator_Priority()
    {
        // Arrange
        var input = "1 + 2 * 3";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 2 * 3);
    }

    [Fact]
    public void Can_Use_Numeric_Literal()
    {
        // Arrange
        var input = "1";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "1 + 2)";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Too many closing braces found");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "(1 + 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing 1 close brace");
    }

    [Fact]
    public void Missing_CloseBrackets_Returns_NotFound()
    {
        // Arrange
        var input = "((1 + 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing 2 close braces");
    }

    [Fact]
    public void Expression_Starting_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "+ 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot start with an operator");
    }

    [Fact]
    public void Expression_Ending_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "1 +";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot end with an operator");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Returns_NotFound()
    {
        // Arrange
        var input = "1 ++ 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Separated_By_Space_Returns_NotFound()
    {
        // Arrange
        var input = "1 + + 2";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Empty_String_Returns_Invalid()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Input cannot be null or empty");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_Invalid()
    {
        // Arrange
        var input = "This string contains the magic \uE002 internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Input cannot contain \uE002, as this is used internally for formatting");
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Int64()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt64).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((long)1 + 1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Decimal()
    {
        // Arrange
        var input = "3.5 + 3.6";

        // Act
        var result = CreateSut(ParseExpressionDelegateDecimal).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((decimal)7.1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Single()
    {
        // Arrange
        var input = "1.4 + 1.3";

        // Act
        var result = CreateSut(ParseExpressionDelegateSingle).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeOfType<float>();
        ((float)result.Value!).ShouldBe(1.4f + 1.3f, 0.1f);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Double()
    {
        // Arrange
        var input = "1.7 + 1.4";

        // Act
        var result = CreateSut(ParseExpressionDelegateDouble).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1.7 + 1.4);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Byte()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateByte).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Add_One_And_One_Using_Short()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt16).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Validate_Add_One_And_One_Using_Int64()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt64).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Validate_One_And_One_Using_Decimal()
    {
        // Arrange
        var input = "3.5 + 3.6";

        // Act
        var result = CreateSut(ParseExpressionDelegateDecimal).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Validate_One_And_One_Using_Single()
    {
        // Arrange
        var input = "1.4 + 1.3";

        // Act
        var result = CreateSut(ParseExpressionDelegateSingle).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Validate_One_And_One_Using_Double()
    {
        // Arrange
        var input = "1.7 + 1.4";

        // Act
        var result = CreateSut(ParseExpressionDelegateDouble).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Validate_One_And_One_Using_Byte()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateByte).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);

    }

    [Fact]
    public void Can_Validate_One_And_One_Using_Short()
    {
        // Arrange
        var input = "1 + 1";

        // Act
        var result = CreateSut(ParseExpressionDelegateInt16).Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Handles_Order_Correctly()
    {
        // Arrange
        var input = "7-16/8*2+8";

        // Act
        var result = CreateSut(ParseExpressionDelegateDouble).Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(11);
    }

    [Fact]
    public void Returns_Error_From_Left_Expression()
    {
        // Arrange
        var input = "bicycle + 1";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Right_Expression()
    {
        // Arrange
        var input = "1 + bicycle";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Nested_Expression()
    {
        // Arrange
        var input = "(1 + bicycle)";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not parse bicycle to integer");
    }

    [Fact]
    public void Returns_Error_From_Aggregation_Operation()
    {
        // Arrange
        var input = "1 / 0";
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Aggregation failed. Error message: Attempted to divide by zero.");
    }

    [Fact]
    public void Returns_Invalid_On_Empty_Input()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var input = default(string?);
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Evaluate(input!, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Empty_Input_Validation()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Validate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Null_Input_Validation()
    {
        // Arrange
        var input = default(string?);
        var sut = CreateSut(ParseExpressionDelegateInt32);

        // Act
        var result = sut.Validate(input!, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    private IMathematicExpressionEvaluator CreateSut(Func<string, IFormatProvider, Result<object?>> dlg)
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton(_variable)
            .AddSingleton<IExpressionEvaluator>(new MyMathematicExpressionParser(dlg))
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<IMathematicExpressionEvaluator>();
    }

    private Result<object?> ParseExpressionDelegateInt32(string arg, IFormatProvider formatProvider)
    {
        if (arg == "$myvariable")
        {
            return _variable.Evaluate(arg[1..], null);
        }
        
        return int.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to integer");
    }

    private static Result<object?> ParseExpressionDelegateInt64(string arg, IFormatProvider formatProvider)
        => long.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to long");

    private static Result<object?> ParseExpressionDelegateDouble(string arg, IFormatProvider formatProvider)
        => double.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to double");

    private static Result<object?> ParseExpressionDelegateSingle(string arg, IFormatProvider formatProvider)
        => float.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to float");

    private static Result<object?> ParseExpressionDelegateDecimal(string arg, IFormatProvider formatProvider)
        => decimal.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to decimal");

    private static Result<object?> ParseExpressionDelegateByte(string arg, IFormatProvider formatProvider)
        => byte.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to byte");

    private static Result<object?> ParseExpressionDelegateInt16(string arg, IFormatProvider formatProvider)
        => short.TryParse(arg, formatProvider, out var result)
            ? Result.Success<object?>(result)
            : Result.Invalid<object?>($"Could not parse {arg} to short");

    public void Dispose()
    {
        _scope?.Dispose();
        _provider?.Dispose();
    }

    private sealed class MyMathematicExpressionParser(Func<string, IFormatProvider, Result<object?>> dlg) : IExpressionEvaluator
    {
        private readonly Func<string, IFormatProvider, Result<object?>> _dlg = dlg;

        public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
            => _dlg.Invoke(expression, formatProvider);

        public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
            => Result.Success(typeof(object));
    }
}
