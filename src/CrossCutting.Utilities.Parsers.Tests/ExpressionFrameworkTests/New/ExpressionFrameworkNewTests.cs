using CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.Current;

namespace CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.New;

public class ExpressionFrameworkNewTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture).Build();

    [Fact]
    public void Can_Validate_ToLowerCaseExpression()
    {
        // Arrange
        var sut = new ToLowerCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToLowerCaseExpressionBuilder()
            .WithExpression(new TypedConstantExpressionBuilder<string>().WithValue("Hello world!"))
            .BuildFunctionCall();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.NoContent);
    }

    [Fact]
    public void Can_Evaluate_ToLowerCaseExpression()
    {
        // Arrange
        var sut = new ToLowerCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new ToLowerCaseExpressionBuilder()
            .WithExpression(new TypedConstantExpressionBuilder<string>().WithValue("Hello world!"))
            .BuildFunctionCall();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("hello world!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), [new ToLowerCaseFunction()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().HaveCount(2);
        functionDescriptors.Single().Results.Should().ContainSingle();
    }
}

[FunctionName(@"ToLowerCase")]
[Description("Converts the expression to lower case")]
[FunctionArgument("Expression", typeof(string), "String to get the lower case for", true)]
[FunctionArgument("Culture", typeof(CultureInfo), "Optional CultureInfo to use", false)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to lower case", "This result will be returned when the expression is of type string")]
// No need to tell what is returned on invalid types of arguments, the framework can do this for you
public class ToLowerCaseFunction : IValidatableFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        //example for OnFailure that has a custom result, with an inner result that contains the details.
        //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
#pragma warning disable CA1308 // Normalize strings to uppercase
        return new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult<string>(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo>(1, "Culture", null))
            .Build()
            .OnFailure(error => Result.Error<object?>([error], "ToLowerCase evaluation failed, see inner results for details"))
            .OnSuccess(results => 
                Result.Success<object?>(results["Culture"].GetValue() is null
                    ? results["Expression"].CastValueAs<string>().ToLowerInvariant()
                    : results["Expression"].CastValueAs<string>().ToLower(results["Culture"].CastValueAs<CultureInfo>())));
#pragma warning restore CA1308 // Normalize strings to uppercase
    }

    // Only needed if you implement IValidatableFunction
    public Result<Type> Validate(FunctionCallContext context)
    {
        // No additional validation needed
        return Result.NoContent<Type>();
    }
}

public record ToLowerCaseExpression : Expression, ITypedExpression<string>
{
    public ToLowerCaseExpression(ITypedExpression<string> expression, ITypedExpression<CultureInfo>? culture)
    {
        Expression = expression;
        Culture = culture;
    }

    public ITypedExpression<string> Expression { get; }
    public ITypedExpression<CultureInfo>? Culture { get; }

    public override Result<object?> Evaluate(object? context)
        => Result.FromExistingResult<object?>(EvaluateTyped(context));

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public ToLowerCaseExpressionBuilder ToTypedBuilder()
    {
        return new ToLowerCaseExpressionBuilder(this);
    }

    public Expression ToUntyped()
    {
        return this;
    }

    public Result<string> EvaluateTyped(object? context)
        => StringExpression.EvaluateCultureExpression(Expression, Culture, context, (culture, value) => value.ToUpper(culture), value => value.ToUpperInvariant());
}

public partial class ToLowerCaseExpressionBuilder : ExpressionBuilder<ToLowerCaseExpressionBuilder, ToLowerCaseExpression>, ITypedExpressionBuilder<string>, IFunctionCallBuilder
{
    private ITypedExpressionBuilder<string> _expression;

    private ITypedExpressionBuilder<CultureInfo>? _culture;

    [Required]
    [ValidateObject]
    public ITypedExpressionBuilder<string> Expression
    {
        get
        {
            return _expression;
        }
        set
        {
            var hasChanged = !EqualityComparer<ITypedExpressionBuilder<string>>.Default.Equals(_expression!, value!);
            _expression = value ?? throw new ArgumentNullException(nameof(value));
            if (hasChanged) HandlePropertyChanged(nameof(Expression));
        }
    }

    public ITypedExpressionBuilder<CultureInfo>? Culture
    {
        get
        {
            return _culture;
        }
        set
        {
            var hasChanged = !EqualityComparer<ITypedExpressionBuilder<CultureInfo>>.Default.Equals(_culture!, value!);
            _culture = value;
            if (hasChanged) HandlePropertyChanged(nameof(Culture));
        }
    }

    public ToLowerCaseExpressionBuilder(ToLowerCaseExpression source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _expression = source.Expression.ToBuilder();
        _culture = source.Culture?.ToBuilder()!;
    }

    public ToLowerCaseExpressionBuilder() : base()
    {
        _expression = new TypedConstantExpressionBuilder<string>()!;
        SetDefaultValues();
    }

    public override ToLowerCaseExpression BuildTyped()
    {
        return new ToLowerCaseExpression(Expression.Build(), Culture?.Build()!);
    }

    partial void SetDefaultValues();

    ITypedExpression<string> ITypedExpressionBuilder<string>.Build()
    {
        return BuildTyped();
    }

    public ToLowerCaseExpressionBuilder WithExpression(ITypedExpressionBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    public ToLowerCaseExpressionBuilder WithExpression(string expression)
    {
        Expression = new TypedConstantExpressionBuilder<string>().WithValue(expression);
        return this;
    }

    public ToLowerCaseExpressionBuilder WithExpression(Func<object?, string> expression)
    {
        Expression = new TypedDelegateExpressionBuilder<string>().WithValue(expression);
        return this;
    }

    public ToLowerCaseExpressionBuilder WithCulture(ITypedExpressionBuilder<CultureInfo>? culture)
    {
        Culture = culture;
        return this;
    }

    public ToLowerCaseExpressionBuilder WithCulture(CultureInfo? culture)
    {
        Culture = culture is null ? null : new TypedConstantExpressionBuilder<CultureInfo>().WithValue(culture);
        return this;
    }

    public ToLowerCaseExpressionBuilder WithCulture(Func<object?, CultureInfo>? culture)
    {
        Culture = culture is null ? null : new TypedDelegateExpressionBuilder<CultureInfo>().WithValue(culture);
        return this;
    }

    public FunctionCall BuildFunctionCall()
    {
        return new FunctionCallBuilder()
            .WithName("ToLowerCase")
            .AddArguments(
                new ExpressionArgumentBuilder<string>().WithExpression(Expression),
                new ExpressionArgumentBuilder<CultureInfo>().WithExpression(Culture))
            .Build();
    }
}

public class ExpressionArgumentBuilder : FunctionCallArgumentBuilder
{
    public ExpressionArgumentBuilder(ExpressionArgument source) : base(source)
    {
        Expression = source?.Expression?.ToBuilder();
    }

    public ExpressionArgumentBuilder() : base()
    {
    }

    public ExpressionBuilder? Expression { get; set; }

    public ExpressionArgumentBuilder WithExpression(ExpressionBuilder? expression)
    {
        Expression = expression;
        return this;
    }

    public override FunctionCallArgument Build()
    {
        return new ExpressionArgument(Expression?.Build());
    }
}

public record ExpressionArgument : FunctionCallArgument
{
    public ExpressionArgument(Expression? expression)
    {
        Expression = expression;
    }

    public Expression? Expression { get; }

    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        return Expression?.Evaluate(context.Context) ?? Result.Success<object?>(null);
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        if (Expression is null)
        {
            // In case of required argument this needs to be Result.Invalid("The Expression argument is required")
            return Result.Continue<Type>();
        }

        var validationResults = new List<ValidationResult>();
        var isValid = Expression.TryValidate(validationResults);
        if (!isValid)
        {
            return Result.Invalid<Type>("Expression validation failed", validationResults.Select(x => new ValidationError(x.ErrorMessage ?? string.Empty, x.MemberNames)));
        }

        // Can we somehow determine if the Expression is ITypedExpression<T>, and if so, return typeof(T)?
        return Result.Continue<Type>();
    }

    public override FunctionCallArgumentBuilder ToBuilder()
    {
        return new ExpressionArgumentBuilder(this);
    }
}

public class ExpressionArgumentBuilder<T> : FunctionCallArgumentBuilder
{
    public ExpressionArgumentBuilder(ExpressionArgument<T> source) : base(source)
    {
        Expression = source?.Expression?.ToBuilder();
    }

    public ExpressionArgumentBuilder() : base()
    {
    }

    public ITypedExpressionBuilder<T>? Expression { get; set; }

    public ExpressionArgumentBuilder<T> WithExpression(ITypedExpressionBuilder<T>? expression)
    {
        Expression = expression;
        return this;
    }

    public override FunctionCallArgument Build()
    {
        return new ExpressionArgument<T>(Expression?.Build());
    }
}

public record ExpressionArgument<T> : FunctionCallArgument
{
    public ExpressionArgument(ITypedExpression<T>? expression)
    {
        Expression = expression;
    }

    public ITypedExpression<T>? Expression { get; }

    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        return Expression?.ToUntyped().Evaluate(context.Context) ?? Result.Success<object?>(null);
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        if (Expression is null)
        {
            // In case of required argument this needs to be Result.Invalid("The Expression argument is required")
            return Result.Continue<Type>();
        }

        var validationResults = new List<ValidationResult>();
        var isValid = Expression.TryValidate(validationResults);
        if (!isValid)
        {
            return Result.Invalid<Type>("Expression validation failed", validationResults.Select(x => new ValidationError(x.ErrorMessage ?? string.Empty, x.MemberNames)));
        }

        return Result.Success<Type>(typeof(T));
    }

    public override FunctionCallArgumentBuilder ToBuilder()
    {
        return new ExpressionArgumentBuilder<T>(this);
    }
}

public interface IFunctionCallBuilder
{
    FunctionCall BuildFunctionCall();
}
