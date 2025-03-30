namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public sealed class MathematicExpressionEvaluatorTests : TestBase, IDisposable
{
    private ServiceProvider? _provider;

    public void Dispose() => _provider?.Dispose();

    [Fact]
    public void Can_Add_One_And_One()
    {
        // Arrange
        var input = "1 + 1";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 1);
    }

    [Fact]
    public void Can_Subtract_One_And_One()
    {
        // Arrange
        var input = "1 - 1";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 - 1);
    }

    [Fact]
    public void Can_Multiply_Two_And_Three()
    {
        // Arrange
        var input = "2 * 3";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2 * 3);
    }

    [Fact]
    public void Can_Divide_Six_By_Two()
    {
        // Arrange
        var input = "6 / 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(6 / 2);
    }

    [Fact]
    public void Can_Use_Power()
    {
        // Arrange
        var input = "2 ^ 4";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((int)Math.Pow(2, 4));
    }

    [Fact]
    public void Can_Use_Modulus()
    {
        // Arrange
        var input = "5 % 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(5 % 2);
    }

    [Fact]
    public void Can_Add_One_And_Two_And_Three()
    {
        // Arrange
        var input = "1 + 2 + 3";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 2 + 3);
    }

    [Fact]
    public void Can_Use_Nested_Formula()
    {
        // Arrange
        var input = "(1 + 2) * 3";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input, evaluator: _provider!.GetRequiredService<IExpressionEvaluator>()));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((1 + 2) * 3);
    }

    [Fact]
    public void Can_Use_Correct_Operator_Priority()
    {
        // Arrange
        var input = "1 + 2 * 3";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1 + 2 * 3);
    }

    [Fact]
    public void Can_Use_Numeric_Literal()
    {
        // Arrange
        var input = "1";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "1 + 2)";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Too many closing braces found");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "(1 + 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing 1 close brace");
    }

    [Fact]
    public void Missing_CloseBrackets_Returns_NotFound()
    {
        // Arrange
        var input = "((1 + 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing 2 close braces");
    }

    [Fact]
    public void Expression_Starting_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "+ 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot start with an operator");
    }

    [Fact]
    public void Expression_Ending_With_Operator_Returns_NotFound()
    {
        // Arrange
        var input = "1 +";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot end with an operator");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Returns_NotFound()
    {
        // Arrange
        var input = "1 ++ 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Expression_Containing_Two_Operators_Next_To_Each_Other_Separated_By_Space_Returns_NotFound()
    {
        // Arrange
        var input = "1 + + 2";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input cannot contain operators without values between them");
    }

    [Fact]
    public void Empty_String_Returns_Invalid()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Input cannot be null or empty");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_Invalid()
    {
        // Arrange
        var input = "This string contains the magic \uE002 internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Input cannot contain \uE002, as this is used internally for formatting");
    }
    
    [Fact]
    public void Handles_Order_Correctly()
    {
        // Arrange
        var input = "7-16/8*2+8";

        // Act
        var result = CreateSut().Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(11);
    }

    [Fact]
    public void Returns_Error_From_Left_Expression()
    {
        // Arrange
        var input = "bicycle + 1";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotSupported);
        result.ErrorMessage.ShouldBe("Unsupported expression: bicycle");
    }

    [Fact]
    public void Returns_Error_From_Right_Expression()
    {
        // Arrange
        var input = "1 + bicycle";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotSupported);
        result.ErrorMessage.ShouldBe("Unsupported expression: bicycle");
    }

    [Fact]
    public void Returns_Error_From_Nested_Expression()
    {
        // Arrange
        var input = "(1 + bicycle)";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.NotSupported);
        result.ErrorMessage.ShouldBe("Unsupported expression: 1 + bicycle");
    }

    [Fact]
    public void Returns_Error_From_Aggregation_Operation()
    {
        // Arrange
        var input = "1 / 0";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Aggregation failed. Error message: Attempted to divide by zero.");
    }

    [Fact]
    public void Returns_Invalid_On_Empty_Input()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var input = default(string?);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Empty_Input_Validation()
    {
        // Arrange
        var input = string.Empty;
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Returns_Invalid_On_Null_Input_Validation()
    {
        // Arrange
        var input = default(string?);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext(input));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    private MathematicExpression CreateSut()
    {
        _provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .BuildServiceProvider(true);
        return _provider.GetServices<IExpression>().OfType<MathematicExpression>().First();
    }
}
