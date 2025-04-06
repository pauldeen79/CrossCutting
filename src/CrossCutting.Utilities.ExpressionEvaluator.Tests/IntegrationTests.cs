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
        var result = sut.Evaluate(CreateContext(expression));

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
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(context)";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, context: "hello world"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    [Fact]
    public void Can_Evaluate_Generic_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyGenericFunction<System.String>()";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

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
        var result = sut.EvaluateTyped<string>(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(null);
    }

    [Fact]
    public void Can_Evaluate_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "\"my string value\"";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression));

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
        var result = sut.Evaluate(CreateContext(expression, context: "replaced"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ToStringWithDefault().ShouldBe("my value with replaced items");
    }

    [Fact]
    public void Can_Validate_Function_With_Wrong_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = sut.Parse(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of function MyFunction failed, see inner results for more details");
        result.PartResults.Count.ShouldBe(1);
        result.PartResults.First().ErrorMessage.ShouldBe("Argument Input is not of type System.String");
    }

    [Fact]
    public void Can_Validate_Function_With_Unknown_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(context)";

        // Act
        var result = sut.Parse(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Validate_Function_With_Parse_Error()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(error)";

        // Act
        var result = sut.Parse(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.PartResults.Count.ShouldBe(1);
        result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
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
            => EvaluateTyped(context).Transform<object?>(x => x);

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
