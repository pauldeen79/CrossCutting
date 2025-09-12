namespace CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.HowItShouldBe;

public class ExpressionFrameworkHowItShouldBeTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

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
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.NoContent);
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
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeOfType<string>();
        result.Value!.ToString().ShouldBe("HELLO WORLD!");
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
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CreateSettings(), null);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeOfType<string>();
        result.Value!.ShouldBe("HELLO WORLD!");
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression_Typed_Without_FunctionCallContext()
    {
        // Act
        var result = ToUpperCaseFunction.Evaluate("Hello world!", null);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeOfType<string>();
        result.Value!.ShouldBe("HELLO WORLD!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), [new ToUpperCaseFunction()], Enumerable.Empty<IGenericFunction>());

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Count.ShouldBe(1);
        functionDescriptors.Single().Arguments.Count.ShouldBe(2);
        functionDescriptors.Single().Results.Count.ShouldBe(1);
    }

    // Note that this scenario only works with strongly-typed function calls
    [Fact]
    public void Can_Convert_Between_FunctionCallBuilder_And_FunctionCall_And_Back_To_FunctionCallBuilder_Again()
    {
        // Arrange
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression(Result.Success("Hello world!"))
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .BuildTyped();

        // Act
        var functionCallBuilder = functionCall.ToTypedBuilder();

        // Assert
        functionCallBuilder.Expression.ShouldBeOfType<ConstantResultArgumentBuilder<string>>();
        ((ConstantResultArgumentBuilder<string>)functionCallBuilder.Expression).Result.Value.ShouldBe("Hello world!");
        functionCallBuilder.CultureInfo.ShouldBeOfType<ConstantArgumentBuilder<CultureInfo?>>();
        ((ConstantArgumentBuilder<CultureInfo?>)functionCallBuilder.CultureInfo).Value.ShouldBe(CultureInfo.InvariantCulture);
    }
}

// *** Generated code
[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument("Expression", typeof(string), "String to get the upper case for", true)]
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

        return new ResultDictionaryBuilder()
            // Note that you can use both GetArgumentValueResult<string> or GetArgumentStringValueResult.
            // This is exactly the same. For Int32, Boolean etc we have special cases, so we might do this for string too.
            // But maybe we'll just skip this, I'm not seeing the difference. (unless the GetArgumentStringValueResult performs a ToString())
            .Add("Expression", () => context.GetArgumentStringValueResult(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo>(1, "Culture", default))
            .Build()
            .OnFailure(OnFailure)
            .OnSuccess(OnSuccess); // in case you need access to the FunctionCallContext: .OnSuccess(OnSuccess(context))
    }

    // When not implementing ITypedFunction<T>, this should be Result<object?> Evaluate instead
    // Note that this can only be generated, if you don't use FunctionCallContext. So this should be an option in the generator.
    // Also note that we can generate it statically, and in this case, we might also just call it Evaluate instead of EvaluateTyped. The Typed suffix is only for method resolution.
    public static Result<string> Evaluate(string expression, CultureInfo? cultureInfo)
    {
        return new ResultDictionaryBuilder()
            .Add("Expression", () => Result.Success(expression))
            .Add("Culture", () => Result.Success(cultureInfo))
            .Build()
            // No OnFailure, because both results are always succesful
            .OnSuccess(OnSuccess); // in case you need access to FunctionCallContext: .OnSuccess(OnSuccess(null!)), but this is tricky because the contect might be null
    }

    // *** Strongly-typed access to arguments
    private static string Expression(IReadOnlyDictionary<string, Result> resultDictionary) => resultDictionary.GetValue<string>("Expression");
    private static CultureInfo? CultureInfo(IReadOnlyDictionary<string, Result> resultsDictionary) => resultsDictionary.TryGetValue<CultureInfo>("Culture");

    // *** Scaffold code, by default throw a NotImplementedException.
    // in case you need access to FuctionCallContext: private static Func<IReadOnlyDictionary<string, Result>, Result<string>> OnSuccess(FunctionCallContext context)
    private static Result<string> OnSuccess(IReadOnlyDictionary<string, Result> results)
    {
        // Note that if you provide a static Evaluate method without FunctionCallContext, then you can't use the function call context (format provider, expression evaluator etc.)
        return Result.Success(Expression(results).ToUpper(CultureInfo(results)));
    }

    /// *** Scaffold code, by default return error
    private static Result OnFailure(Result error)
    {
        // If you want to return the error unchanged, just use return error to let it bubble up (default behavior?)
        // Or, maybe using settings you can choose the behavior of this method. (bubble, wrap, skip the OnFailure entirely, not implemented exception)
        return error;
        ///example for custom error: return Result.Error([error], "ToUpperCase evaluation failed, see inner results for details");
    }

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
    public IFunctionCallArgumentBuilder<string> Expression { get; set; }
    public IFunctionCallArgumentBuilder<CultureInfo?> CultureInfo { get; set; }

    public ToUpperCaseFunctionCallBuilder()
    {
        // Same functionality as in ClassFramework.Pipelines: When it's a non-nullable string, then assign String.Empty. (and also, initialize collections and required builder-typed properties to new instances)
        // Not sure if you can plug into the Builder pipeline to customize this...
        Expression = new ConstantArgumentBuilder<string>().WithValue(string.Empty);
        CultureInfo = new ConstantArgumentBuilder<CultureInfo?>();
        // Alternative:
        ///CultureInfo = new EmptyArgumentBuilder<CultureInfo?>();
    }

    public ToUpperCaseFunctionCallBuilder WithExpression(IFunctionCallArgumentBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    // Need this overload because implicit operators can't be created on interfaces :(
    public ToUpperCaseFunctionCallBuilder WithExpression(ConstantArgumentBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    // Need this overload because implicit operators can't be created on interfaces :(
    public ToUpperCaseFunctionCallBuilder WithExpression(ConstantResultArgumentBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    public ToUpperCaseFunctionCallBuilder WithCultureInfo(IFunctionCallArgumentBuilder<CultureInfo?> cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        CultureInfo = cultureInfo;
        return this;
    }

    // Need this overload because implicit operators can't be created on interfaces :(
    public ToUpperCaseFunctionCallBuilder WithCultureInfo(ConstantArgumentBuilder<CultureInfo?> cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        CultureInfo = cultureInfo;
        return this;
    }

    // Need this overload because implicit operators can't be created on interfaces :(
    public ToUpperCaseFunctionCallBuilder WithCultureInfo(ConstantResultArgumentBuilder<CultureInfo?> cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        CultureInfo = cultureInfo;
        return this;
    }

    // Only works if you use strongly-typed FunctionCall
    public ToUpperCaseFunctionCall BuildTyped()
    {
        return new ToUpperCaseFunctionCall(Expression.Build(), CultureInfo.Build());
    }

    public FunctionCall Build()
    {
        // This definitely doesn't work out of the box.
        // You have to customize this also.

        // Strongly-typed FunctionCall: (maybe too much overhead)
        return new ToUpperCaseFunctionCall(Expression.Build(), CultureInfo.Build());

        // Generic FunctionCall:
        ///return new FunctionCallBuilder()
        ///    .WithName(@"ToUpperCase")
        ///    .AddArguments(Expression, CultureInfo)
        ///    .Build();
    }
}

// *** Generated code (optional)
public record ToUpperCaseFunctionCall : FunctionCall, IBuildableEntity<ToUpperCaseFunctionCallBuilder> // Inheriting from IBuildableEntity<T> is optional.
{
    public ToUpperCaseFunctionCall(IFunctionCallArgument<string> expression, IFunctionCallArgument<CultureInfo?> cultureInfo) : base("ToUpperCase", [expression, cultureInfo], Enumerable.Empty<IFunctionCallTypeArgument>())
    {
    }

    // Only generate this when inheriting from IBuildableEntity<T>
    ToUpperCaseFunctionCallBuilder IBuildableEntity<ToUpperCaseFunctionCallBuilder>.ToBuilder()
    {
        return ToTypedBuilder();
    }

    public ToUpperCaseFunctionCallBuilder ToTypedBuilder()
    {
        return new ToUpperCaseFunctionCallBuilder()
            .WithExpression(((IFunctionCallArgument<string>)Arguments.ElementAt(0)).ToBuilder())
            .WithCultureInfo(((IFunctionCallArgument<CultureInfo?>)Arguments.ElementAt(1)).ToBuilder());
    }
}
