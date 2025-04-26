namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    [Fact]
    public void Can_Evaluate_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-1 + 1 + 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1);
    }

    [Fact]
    public void Can_Evaluate_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(context)";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, context: "hello world", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    [Fact]
    public void Can_Evaluate_Property_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "context.Length";

        // Act
        var result = sut.Evaluate(CreateContext(expression, context: "hello world", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Length);
    }

    [Fact]
    public void Can_Evaluate_Method_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "context.ToString()";

        // Act
        var result = sut.Evaluate(CreateContext(expression, context: "hello world", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".ToString());
    }

    [Fact]
    public void Can_Evaluate_Generic_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyGenericFunction<System.String>()";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Can_Evaluate_Primitive_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "null";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(null);
    }

    [Fact]
    public void Can_Evaluate_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "13";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public void Can_Evaluate_Negative_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-13";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(-13);
    }

    [Fact]
    public void Can_Evaluate_Negate_Boolean_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "!false";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "\"my string value\"";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("my string value");
    }

    [Fact]
    public void Can_Evaluate_Formattable_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "$\"my value with {context} items\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression, context: "replaced", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ToStringWithDefault().ShouldBe("my value with replaced items");
    }

    [Theory]
    [InlineData("(1 + 1 + 1) == 3 && \"true\" == \"true\"")]
    [InlineData("(1 == 1) && 2 == 2")]
    [InlineData("MyFunction(\"a\") != MyFunction(\"b\")")]
    [InlineData("MyFunction(\"+\") != MyFunction(\"-\")")]
    public void Can_Evaluate_Operator_Expression(string expression)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Operator_Expression_With_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("!true", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Can_Evaluate_Operator_Expression_With_Double_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("!!true", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToString_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToString(true)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true.ToString());
    }

    [Fact]
    public void Can_Evaluate_Expression_With_IsNull_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("IsNull(\"some value that is not null\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToCamelCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToCamelCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToCamelCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToLowerCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToLowerCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToLower(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToPascalCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToPascalCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToPascalCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToUpperCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToUpperCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToUpper(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_Date_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("Date(2025, 1, 1)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_DateTime_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("DateTime(2025, 1, 1, 12, 13, 14)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 12, 13, 14, DateTimeKind.Unspecified));
    }

    [Fact]
    public void Can_Parse_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "(-1 + 1) + 1";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Wrong_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of function MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Argument Input is not of type System.String");
    }

    [Fact]
    public void Can_Skip_Validation_On_ArgumentTypes_Using_Setting()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = sut.Parse(CreateContext(expression, settings: new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(false), evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Unknown_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(context)";
        var context = new DeferredResultDictionaryBuilder<object?>()
            .Add("context", Result.NoContent<object?>)
            .Build();

        // Act
        var result = sut.Parse(CreateContext(expression, context: context, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Parse_Error()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(error)";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of function MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Unknown expression type found in fragment: error");
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddSingleton<IFunction, MyFunction>()
            .AddSingleton<IGenericFunction, MyGenericFunction>()
            .BuildServiceProvider();
    }

    private ServiceProvider Provider { get; set; }

    private IExpressionEvaluator CreateSut() => Provider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose() => Provider.Dispose();

    [FunctionName("MyFunction")]
    [FunctionArgument("Input", typeof(string))]
    private sealed class MyFunction : IFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
            => EvaluateTyped(context).TryCastAllowNull<object?>();

        public Result<string> EvaluateTyped(FunctionCallContext context)
            => new ResultDictionaryBuilder()
                .Add("Input", () => context.GetArgumentValueResult<string>(0, "Input"))
                .Build()
                .OnSuccess(results => Result.Success(results.GetValue<string>("Input").ToUpperInvariant()));
    }

    [FunctionName("MyGenericFunction")]
    [FunctionTypeArgument("T", "Type argument to use")]
    private sealed class MyGenericFunction : IGenericFunction
    {
        public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
            => Result.Success<object?>(typeof(T));
    }
}
