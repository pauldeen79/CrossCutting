namespace CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.HowItShouldBe;

public class ExpressionFrameworkHowItShouldBeTests
{
    [Fact]
    public void Can_Validate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.NoContent);
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression_Typed()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression(Result.Success("Hello world!"))
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression_Typed_Without_FunctionCallContext()
    {
        // Act
        var result = ToUpperCaseFunction.Evaluate("Hello world!", null);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), [new ToUpperCaseFunction()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().HaveCount(2);
        functionDescriptors.Single().Results.Should().ContainSingle();
    }
}

// *** Generated code
[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument("Expression", typeof(string), "String to get the upper c00ase for", true)]
[FunctionArgument("Culture", typeof(CultureInfo), "Optional CultureInfo to use", false)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to upper case", "This result will be returned when the expression is of type string")]
// No need to tell what is returned on invalid types of arguments, the framework can do this for you
public class ToUpperCaseFunction : ITypedFunction<string>, IValidatableFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        return EvaluateTyped(context).Transform<object?>(value => value);
    }

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        /// *** With strongly-typed function call context:
        ///var typedContext = new ToUpperCaseFunctionCallContext(context);
        ///return typedContext
        ///    //example for OnFailure that has a custom result, with an inner result that contains the details.
        ///    //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
        ///    .OnFailure(OnFailure)
        ///    .OnSuccess(OnSuccess(typedContext));

        // *** Without strongly-typed function call context:
        return new ResultDictionaryBuilder()
            // Note that you can use both GetArgumentValueResult<string> or GetArgumentStringValueResult.
            // This is exactly the same. For Int32, Boolean etc we have special cases, so we might do this for string too.
            // But maybe we'll just skip this, I'm not seeing the difference. (unless the GetArgumentStringValueResult performs a ToString())
            .Add("Expression", () => context.GetArgumentStringValueResult(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo>(1, "Culture", default))
            .Build()
            .OnFailure(OnFailure)
            .OnSuccess(OnSuccess(context));
    }

    // When not implementing ITypedFunction<T>, this should be Result<object?> Evaluate instead
    // Note that this can only be generated, if you don't use FunctionCallContext. So this should be an option in the generator.
    // Also note that we can generate it statically, and in this case, we might also just call it Evaluate instead of EvaluateTyped. The Typed suffix is only for method resolution.
    public static Result<string> Evaluate(string expression, CultureInfo? cultureInfo)
    {
        // *** Without strongly-typed function call context:
        return new ResultDictionaryBuilder()
            // Note that you can use both GetArgumentValueResult<string> or GetArgumentStringValueResult.
            // This is exactly the same. For Int32, Boolean etc we have special cases, so we might do this for string too.
            // But maybe we'll just skip this, I'm not seeing the difference. (unless the GetArgumentStringValueResult performs a ToString())
            .Add("Expression", () => Result.Success(expression))
            .Add("Culture", () => Result.Success(cultureInfo))
            .Build()
            // No OnFailure, because both results are always succesful
            .OnSuccess(OnSuccess(null!)); // We can force null, as long as you don't use the FunctionCallContext
    }

    // *** Strongly-typed access to arguments
    private static string Expression(Dictionary<string, Result> resultDictionary) => resultDictionary.GetValue<string>("Expression");
    private static CultureInfo? CultureInfo(Dictionary<string, Result> resultsDictionary, CultureInfo? defaultValue) => resultsDictionary.TryGetValue("Culture", defaultValue);

    // *** Scaffold code, by default throw a NotImplementedException.
    private static Func<Dictionary<string, Result>, Result<string>> OnSuccess(FunctionCallContext context)
    {
        return results => Result.Success(Expression(results).ToUpper(CultureInfo(results, context?.FormatProvider.ToCultureInfo())));
    }

    private static Result OnFailure(Result error)
    {
        // If you want to return the error unchanged, just use return error to let it bubble up (default behavior?)
        return error;
        ///example for custom error: return Result.Error([error], "ToUpperCase evaluation failed, see inner results for details");
    }
    /// *** Scaffold code, by default throw a NotImplementedException
    ///private static Func<Dictionary<string, Result>, Result<string>> OnSuccess(ToUpperCaseFunctionCallContext context)
    ///{
    ///    return results => Result.Success(context.Expression().ToUpper(context.CultureInfo(context.FormatProvider.ToCultureInfo())));
    ///}

    /// *** Scaffold code, by default return error
    ///private static Result OnFailure(Result error)
    ///{
    ///    // If you want to return the error unchanged, just use return error to let it bubble up (default behavior)
    ///    // Or, maybe using settings you can choose the behavior of this method. (bubble, wrap, skip the OnFailure entirely, not implemented exception)
    ///    return error;
    ///    ///example for custom error: return Result.Error([error], "ToUpperCase evaluation failed, see inner results for details");
    ///}

    // *** Scaffold code, by default return Result.Success()
    // Only if you implement IValidatableFunction
    public Result<Type> Validate(FunctionCallContext context)
    {
        // No additional validation needed (default behavior)
        // Or, maybe using settings you can choose whether to return Result.Success(), or throw a NotImplementedException for clarity.
        return Result.NoContent<Type>();
    }
}

// *** Generated code
public class ToUpperCaseFunctionCallBuilder : IBuilder<FunctionCall> // Inheriting from IBuilder<T> is optional. Maybe we can add ITypedBuilder<TBase, TTyped> to Abstractions as well?
{
    // You might be able to re-use the default builder pipeline, but then you have to do some typemapping.
    public FunctionCallArgumentBuilder<string> Expression { get; set; }
    public FunctionCallArgumentBuilder<CultureInfo?> CultureInfo { get; set; }

    public ToUpperCaseFunctionCallBuilder()
    {
        // Same functionality as in ClassFramework.Pipelines: When it's a non-nullable string, then assign String.Empty. (and also, initialize collections and required builder-typed properties to new instances)
        // Not sure if you can plug into the Builder pipeline to customize this...
        Expression = new ConstantArgumentBuilder<string>().WithValue(string.Empty);
        CultureInfo = new ConstantArgumentBuilder<CultureInfo?>();
    }

    public ToUpperCaseFunctionCallBuilder WithExpression(FunctionCallArgumentBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    public ToUpperCaseFunctionCallBuilder WithCultureInfo(FunctionCallArgumentBuilder<CultureInfo?> cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        CultureInfo = cultureInfo;
        return this;
    }

    // Only works if you use strongly-typed FunctionCall
    ///public ToUpperCaseFunctionCall BuildTyped()
    ///{
    ///    return new ToUpperCaseFunctionCall(Expression.BuildTyped(), CultureInfo.BuildTyped());
    ///}

    public FunctionCall Build()
    {
        // This definitely doesn't work out of the box.
        // You have to customize this also.

        // Strongly-typed FunctionCall: (maybe too much overhead)
        ///return new ToUpperCaseFunctionCall(Expression.BuildTyped(), CultureInfo.BuildTyped());

        // Generic FunctionCall:
        return new FunctionCallBuilder()
            .WithName(@"ToUpperCase")
            .AddArguments(Expression, CultureInfo)
            .Build();
    }
}

// *** Generated code (optional)
///public record ToUpperCaseFunctionCall : FunctionCall
///{
///    public ToUpperCaseFunctionCall(FunctionCallArgument<string> expression, FunctionCallArgument<CultureInfo?> cultureInfo) : base("ToUpperCase", new FunctionCallArgument[] { expression, cultureInfo } )
///    {
///    }
///
///    protected ToUpperCaseFunctionCall(FunctionCall original) : base(original)
///    {
///    }
///}

// *** Generated code
public class ToUpperCaseFunctionCallContext : FunctionCallContext, IResultDictionaryContainer
{
    public Dictionary<string, Result> Results { get; }

    public ToUpperCaseFunctionCallContext(FunctionCallContext context) : base(context?.FunctionCall ?? throw new ArgumentNullException(nameof(context)), context.FunctionEvaluator, context.ExpressionEvaluator, context.FormatProvider, context.Context)
    {
        Results = new ResultDictionaryBuilder()
            // Note that you can use both GetArgumentValueResult<string> or GetArgumentStringValueResult.
            // This is exactly the same. For Int32, Boolean etc we have special cases, so we might do this for string too.
            // But maybe we'll just skip this, I'm not seeing the difference. (unless the GetArgumentStringValueResult performs a ToString())
            .Add("Expression", () => GetArgumentStringValueResult(0, "Expression"))
            .Add("Culture", () => GetArgumentValueResult<CultureInfo>(1, "Culture", default))
            .Build();
    }

    // Strongly-typed access to arguments
    public string Expression() => Results.GetValue<string>("Expression");
    // For optional arguments, generate two overloads (with and without default value)
    public CultureInfo? CultureInfo() => Results.TryGetValue<CultureInfo>("Culture");
    public CultureInfo? CultureInfo(CultureInfo? defaultValue) => Results.TryGetValue("Culture", defaultValue);
}
