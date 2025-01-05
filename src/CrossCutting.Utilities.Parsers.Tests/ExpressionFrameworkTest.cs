namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class ExpressionFrameworkTest
{
    [Fact]
    public void Can_Parse_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseExpressionResolver();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new FunctionCallBuilder().WithName("ToUpperCase").AddArguments(new LiteralArgumentBuilder().WithValue("Hello world!")).Build();

        // Act
        var result = sut.Parse(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<ToUpperCaseExpression>();
    }

    [Fact]
    public void Can_Validate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseExpressionResolver();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new FunctionCallBuilder().WithName("ToUpperCase").AddArguments(new LiteralArgumentBuilder().WithValue("Hello world!")).Build();

        // Act
        var result = sut.Validate(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseExpressionResolver();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new FunctionCallBuilder().WithName("ToUpperCase").AddArguments(new LiteralArgumentBuilder().WithValue("Hello world!")).Build();

        // Act
        var result = sut.Evaluate(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider([new ToUpperCaseExpressionResolver()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().ContainSingle();
        functionDescriptors.Single().Results.Should().HaveCount(2);
    }
}

public partial record ToUpperCaseExpression : Expression, ITypedExpression<string>
{
    public override Result<object?> Evaluate(object? context)
        => Result.FromExistingResult<object?>(EvaluateTyped(context));

    public Result<string> EvaluateTyped(object? context)
        => StringExpression.EvaluateCultureExpression(Expression, Culture, context, (culture, value) => value.ToUpper(culture), value => value.ToUpperInvariant());

    [Required]
    [ValidateObject]
    public ITypedExpression<string> Expression
    {
        get;
    }

    public ITypedExpression<CultureInfo>? Culture
    {
        get;
    }

    public ToUpperCaseExpression(ITypedExpression<string> expression, ITypedExpression<CultureInfo>? culture) : base()
    {
        this.Expression = expression;
        this.Culture = culture;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public ToUpperCaseExpressionBuilder ToTypedBuilder()
    {
        return new ToUpperCaseExpressionBuilder(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.Success(Expression.ToUntyped());
    }

    public Expression ToUntyped()
    {
        return this;
    }
}

public partial class ToUpperCaseExpressionBuilder : ExpressionBuilder<ToUpperCaseExpressionBuilder, ToUpperCaseExpression>, ITypedExpressionBuilder<string>
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
            bool hasChanged = !EqualityComparer<ITypedExpressionBuilder<string>>.Default.Equals(_expression!, value!);
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
            bool hasChanged = !EqualityComparer<ITypedExpressionBuilder<CultureInfo>>.Default.Equals(_culture!, value!);
            _culture = value;
            if (hasChanged) HandlePropertyChanged(nameof(Culture));
        }
    }

    public ToUpperCaseExpressionBuilder(ToUpperCaseExpression source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _expression = source.Expression.ToBuilder();
        _culture = source.Culture?.ToBuilder()!;
    }

    public ToUpperCaseExpressionBuilder() : base()
    {
        _expression = new TypedConstantExpressionBuilder<string>()!;
        SetDefaultValues();
    }

    public override ToUpperCaseExpression BuildTyped()
    {
        return new ToUpperCaseExpression(Expression.Build(), Culture?.Build()!);
    }

    partial void SetDefaultValues();

    ITypedExpression<string> ITypedExpressionBuilder<string>.Build()
    {
        return BuildTyped();
    }

    public ToUpperCaseExpressionBuilder WithExpression(ITypedExpressionBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }
    
    public ToUpperCaseExpressionBuilder WithExpression(string expression)
    {
        Expression = new TypedConstantExpressionBuilder<string>().WithValue(expression);
        return this;
    }

    public ToUpperCaseExpressionBuilder WithExpression(Func<object?, string> expression)
    {
        Expression = new TypedDelegateExpressionBuilder<string>().WithValue(expression);
        return this;
    }

    public ToUpperCaseExpressionBuilder WithCulture(ITypedExpressionBuilder<CultureInfo>? culture)
    {
        Culture = culture;
        return this;
    }

    public ToUpperCaseExpressionBuilder WithCulture(CultureInfo? culture)
    {
        Culture = culture is null ? null : new TypedConstantExpressionBuilder<CultureInfo>().WithValue(culture);
        return this;
    }

    public ToUpperCaseExpressionBuilder WithCulture(Func<object?, CultureInfo>? culture)
    {
        Culture = culture is null ? null : new TypedDelegateExpressionBuilder<CultureInfo>().WithValue(culture);
        return this;
    }
}

[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument(nameof(Expression), typeof(string), "String to get the upper case for", true)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to upper case", "This result will be returned when the expression is of type string")]
[FunctionResult(ResultStatus.Invalid, "Expression must be of type string")]
public class ToUpperCaseExpressionResolver : ExpressionResolverBase
{
    protected override Result<Expression> DoParse(FunctionCall functionCall, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
    {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        return Result.Success<Expression>(new ToUpperCaseExpression(
            functionCall.GetArgumentStringValueExpression(0, @"Expression", evaluator, parser, formatProvider, context),
            functionCall.GetArgumentValueExpression<CultureInfo>(1, @"Culture", evaluator, parser, formatProvider, context, default)));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }
}

public abstract partial record Expression
{
    protected Expression()
    {
    }

    public abstract ExpressionBuilder ToBuilder();

    public Result<object?> Evaluate() => Evaluate(null);

    public abstract Result<object?> Evaluate(object? context);

    public abstract Result<Expression> GetSingleContainedExpression();
}

public abstract partial class ExpressionBuilder : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected ExpressionBuilder(Expression source)
    {
        ArgumentNullException.ThrowIfNull(source);
    }

    protected ExpressionBuilder()
    {
        SetDefaultValues();
    }

    public abstract Expression Build();

    partial void SetDefaultValues();

    protected void HandlePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public abstract partial class ExpressionBuilder<TBuilder, TEntity> : ExpressionBuilder
    where TEntity : Expression
    where TBuilder : ExpressionBuilder<TBuilder, TEntity>
{
    protected ExpressionBuilder(Expression source) : base(source)
    {
    }

    protected ExpressionBuilder() : base()
    {
    }

    public override Expression Build()
    {
        return BuildTyped();
    }

    public abstract TEntity BuildTyped();
}

public partial record ConstantExpression : Expression
{
    public override Result<object?> Evaluate(object? context)
        => Result.Success(Value);
    
    public object? Value
    {
        get;
    }

    public ConstantExpression(object? value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public ConstantExpressionBuilder ToTypedBuilder()
    {
        return new ConstantExpressionBuilder(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial record ConstantResultExpression : Expression
{
    public override Result<object?> Evaluate(object? context)
        => Result.FromExistingResult<object?>(Value);
    
    [Required]
    [ValidateObject]
    public Result Value
    {
        get;
    }

    public ConstantResultExpression(Result value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public ConstantResultExpressionBuilder ToTypedBuilder()
    {
        return new ConstantResultExpressionBuilder(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial class ConstantResultExpressionBuilder : ExpressionBuilder<ConstantResultExpressionBuilder, ConstantResultExpression>
{
    private Result _value;

    [Required]
    [ValidateObject]
    public Result Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<Result>.Default.Equals(_value!, value!);
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public ConstantResultExpressionBuilder(ConstantResultExpression source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public ConstantResultExpressionBuilder() : base()
    {
        _value = default(Result)!;
        SetDefaultValues();
    }

    public override ConstantResultExpression BuildTyped()
    {
        return new ConstantResultExpression(Value);
    }

    partial void SetDefaultValues();

    public ConstantResultExpressionBuilder WithValue(Result value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
        return this;
    }
}

public partial record DelegateExpression : Expression
{
    public override Result<object?> Evaluate(object? context)
        => Result.Success(Value.Invoke(context));

    [Required]
    public Func<object?, object?> Value
    {
        get;
    }

    public DelegateExpression(Func<object?, object?> value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public DelegateExpressionBuilder ToTypedBuilder()
    {
        return new DelegateExpressionBuilder(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial record TypedConstantExpression<T> : Expression, ITypedExpression<T>
{
    public override Result<object?> Evaluate(object? context)
        => Result.Success<object?>(Value);

    public Result<T> EvaluateTyped(object? context)
        => Result.Success(Value);

    public Expression ToUntyped()
        => new ConstantExpression(Value);

    public T Value
    {
        get;
    }

    public TypedConstantExpression(T value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public TypedConstantExpressionBuilder<T> ToTypedBuilder()
    {
        return new TypedConstantExpressionBuilder<T>(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial record TypedConstantResultExpression<T> : Expression, ITypedExpression<T>
{
    public override Result<object?> Evaluate(object? context)
        => Result.FromExistingResult<object?>(Value);

    public Result<T> EvaluateTyped(object? context)
        => Value;

    public Expression ToUntyped()
        => new ConstantExpression(Value.Value);
    
    [Required]
    public Result<T> Value
    {
        get;
    }

    public TypedConstantResultExpression(Result<T> value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public TypedConstantResultExpressionBuilder<T> ToTypedBuilder()
    {
        return new TypedConstantResultExpressionBuilder<T>(this);
    }

    public override Result<Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial record TypedDelegateExpression<T> : Expression, ITypedExpression<T>
{
    public override Result<object?> Evaluate(object? context)
        => Result.Success<object?>(Value.Invoke(context));

    public Result<T> EvaluateTyped(object? context)
        => Result.Success(Value.Invoke(context));

    public Expression ToUntyped()
        => new DelegateExpression(context => Value.Invoke(context));

    [Required]
    public Func<object?, T> Value
    {
        get;
    }

    public TypedDelegateExpression(Func<object?, T> value) : base()
    {
        this.Value = value;
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override ExpressionBuilder ToBuilder()
    {
        return ToTypedBuilder();
    }

    public TypedDelegateExpressionBuilder<T> ToTypedBuilder()
    {
        return new TypedDelegateExpressionBuilder<T>(this);
    }

    public override Result< Expression> GetSingleContainedExpression()
    {
        return Result.NotFound<Expression>();
    }
}

public partial class ConstantExpressionBuilder : ExpressionBuilder<ConstantExpressionBuilder, ConstantExpression>
{
    private object? _value;

    public object? Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<object>.Default.Equals(_value!, value!);
            _value = value;
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public ConstantExpressionBuilder(ConstantExpression source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public ConstantExpressionBuilder() : base()
    {
        SetDefaultValues();
    }

    public override ConstantExpression BuildTyped()
    {
        return new ConstantExpression(Value);
    }

    partial void SetDefaultValues();

    public ConstantExpressionBuilder WithValue(object? value)
    {
        Value = value;
        return this;
    }
}

public partial class DelegateExpressionBuilder : ExpressionBuilder<DelegateExpressionBuilder, DelegateExpression>
{
    private Func<object?, object?> _value;

    [Required]
    public Func<object?, object?> Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<Func<object?, object?>>.Default.Equals(_value!, value!);
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public DelegateExpressionBuilder(DelegateExpression source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public DelegateExpressionBuilder() : base()
    {
        _value = default(Func<object?, object?>)!;
        SetDefaultValues();
    }

    public override DelegateExpression BuildTyped()
    {
        return new DelegateExpression(Value);
    }

    partial void SetDefaultValues();

    public DelegateExpressionBuilder WithValue(Func<object?, object?> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
        return this;
    }
}

public partial class TypedConstantExpressionBuilder<T> : ExpressionBuilder<TypedConstantExpressionBuilder<T>, TypedConstantExpression<T>>, ITypedExpressionBuilder<T>
{
    private T _value;

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<T>.Default.Equals(_value!, value!);
            _value = value;
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public TypedConstantExpressionBuilder(TypedConstantExpression<T> source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public TypedConstantExpressionBuilder() : base()
    {
        _value = default(T)!;
        SetDefaultValues();
    }

    public override TypedConstantExpression<T> BuildTyped()
    {
        return new TypedConstantExpression<T>(Value);
    }

    private void SetDefaultValues()
    {
        if (typeof(T) == typeof(string))
        {
            GetType().GetProperty(nameof(Value))!.SetValue(this, string.Empty);
        }
    }

    public TypedConstantExpressionBuilder<T> WithValue(T value)
    {
        Value = value;
        return this;
    }

    ITypedExpression<T> ITypedExpressionBuilder<T>.Build()
    {
        return BuildTyped();
    }
}

public partial class TypedConstantResultExpressionBuilder<T> : ExpressionBuilder<TypedConstantResultExpressionBuilder<T>, TypedConstantResultExpression<T>>, ITypedExpressionBuilder<T>
{
    private Result<T> _value;

    [Required]
    public Result<T> Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<Result<T>>.Default.Equals(_value!, value!);
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public TypedConstantResultExpressionBuilder(TypedConstantResultExpression<T> source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public TypedConstantResultExpressionBuilder() : base()
    {
        _value = default(Result<T>)!;
        SetDefaultValues();
    }

    public override TypedConstantResultExpression<T> BuildTyped()
    {
        return new TypedConstantResultExpression<T>(Value);
    }

    partial void SetDefaultValues();

    public TypedConstantResultExpressionBuilder<T> WithValue(Result<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
        return this;
    }

    ITypedExpression<T> ITypedExpressionBuilder<T>.Build()
    {
        return BuildTyped();
    }
}

public partial class TypedDelegateExpressionBuilder<T> : ExpressionBuilder<TypedDelegateExpressionBuilder<T>, TypedDelegateExpression<T>>, ITypedExpressionBuilder<T>
{
    private Func<object?, T> _value;

    [Required]
    public Func<object?, T> Value
    {
        get
        {
            return _value;
        }
        set
        {
            bool hasChanged = !EqualityComparer<Func<object?, T>>.Default.Equals(_value!, value!);
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (hasChanged) HandlePropertyChanged(nameof(Value));
        }
    }

    public TypedDelegateExpressionBuilder(TypedDelegateExpression<T> source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _value = source.Value;
    }

    public TypedDelegateExpressionBuilder() : base()
    {
        _value = default(Func<object?, T>)!;
        SetDefaultValues();
    }

    public override TypedDelegateExpression<T> BuildTyped()
    {
        return new TypedDelegateExpression<T>(Value);
    }

    partial void SetDefaultValues();

    public TypedDelegateExpressionBuilder<T> WithValue(Func<object?, T> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
        return this;
    }

    ITypedExpression<T> ITypedExpressionBuilder<T>.Build()
    {
        return BuildTyped();
    }
}

public abstract class ExpressionResolverBase : IFunction, IExpressionResolver
{
    public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        var result = Parse(functionCall, functionEvaluator, expressionEvaluator, formatProvider, context);

        return result.IsSuccessful() && result.Status != ResultStatus.Continue
            ? result.Value?.Evaluate(context) ?? Result.Success<object?>(null)
            : Result.FromExistingResult<object?>(result);
    }

    public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        return Parse(functionCall, functionEvaluator, expressionEvaluator, formatProvider, context);
    }

    public Result<Expression> Parse(FunctionCall functionCall, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
    {
        functionCall = ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));

        return DoParse(functionCall, evaluator, parser, formatProvider, context);
    }

    protected abstract Result<Expression> DoParse(FunctionCall functionCall, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context);

    protected static Result<Expression> ParseTypedExpression(Type expressionType, int index, string argumentName, FunctionCall functionCall, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
    {
        expressionType = ArgumentGuard.IsNotNull(expressionType, nameof(expressionType));
        functionCall = ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));

        var typeResult = functionCall.Name.GetGenericTypeResult();
        if (!typeResult.IsSuccessful())
        {
            return Result.FromExistingResult<Expression>(typeResult);
        }

        var valueResult = functionCall.GetArgumentValueResult(index, argumentName, formatProvider, context, evaluator, parser);
        if (!valueResult.IsSuccessful())
        {
            return Result.FromExistingResult<Expression>(valueResult);
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return Result.Success((Expression)Activator.CreateInstance(expressionType.MakeGenericType(typeResult.Value!), valueResult.Value)!);
        }
        catch (Exception ex)
        {
            return Result.Invalid<Expression>($"Could not create {expressionType.Name.Replace("`1", string.Empty, StringComparison.Ordinal)}. Error: {ex.Message}");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}

public interface ITypedExpression<T> : IUntypedExpressionProvider
{
    Result<T> EvaluateTyped(object? context);
}

public interface ITypedExpressionBuilder<T>
{
    ITypedExpression<T> Build();
}

public interface IUntypedExpressionProvider
{
    Expression ToUntyped();
}

public interface IExpressionResolver
{
    Result<Expression> Parse(FunctionCall functionCall, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context);
}

public static class StringExpression
{
    public static Result<string> EvaluateCultureExpression(
        ITypedExpression<string> expression,
        ITypedExpression<CultureInfo>? cultureExpression,
        object? context,
        Func<CultureInfo, string, string> cultureDelegate,
        Func<string, string> noCultureDelegate)
    {
        if (cultureExpression is null)
        {
            return expression.EvaluateTypedWithTypeCheck(context).Either(
                error => error,
                success => Result.Success(noCultureDelegate(success.Value!))
            );
        }

        var cultureResult = cultureExpression.EvaluateTyped();
        if (!cultureResult.IsSuccessful())
        {
            return Result.FromExistingResult<string>(cultureResult);
        }

        return expression.EvaluateTypedWithTypeCheck(context).Either(
                error => error,
                success => Result.Success(cultureDelegate(cultureResult.Value!, success.Value!))
            );
    }
}

public static class ExpressionExtensions
{
    public static Result<object?> EvaluateWithNullCheck(this Expression instance, object? context, string? errorMessage = null)
        => instance.Evaluate(context).Transform(result => result.IsSuccessful() && result.Value is null
            ? Result.Invalid<object?>(errorMessage.WhenNullOrEmpty("Expression cannot be empty"))
            : result);

    public static Result<T> EvaluateTyped<T>(this Expression instance, object? context = null, string? errorMessage = null)
        => instance is ITypedExpression<T> typedExpression
            ? typedExpression.EvaluateTyped(context)
            : instance.Evaluate(context).TryCast<T>(errorMessage);

    public static Result<T> EvaluateTypedWithTypeCheck<T>(this ITypedExpression<T> instance, object? context = null, string? errorMessage = null)
        => instance.EvaluateTyped(context).Transform(result => result.IsSuccessful() && result.Value is T t
            ? Result.FromExistingResult(result, t) // use FromExistingResult because status might be Ok, Continue or another successful status
            : result.Either(
                error => error,
                _ => Result.Invalid<T>(CreateInvalidTypeErrorMessage<T>(errorMessage))));

    public static string CreateInvalidTypeErrorMessage<T>(string? errorMessage = null)
        => errorMessage.WhenNullOrEmpty(() => $"Expression is not of type {GetTypeName(typeof(T))}");

    private static string? GetTypeName(Type type)
    {
        if (type == typeof(string))
        {
            return "string";
        }

        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            return "enumerable";
        }

        return type.FullName;
    }
}

public static class TypedExpressionExtensions
{
    public static Result<T> EvaluateTyped<T>(this ITypedExpression<T> instance)
        => instance.EvaluateTyped(null);

    public static ITypedExpressionBuilder<T> ToBuilder<T>(this ITypedExpression<T> source)
        => source switch
        {
            TypedConstantExpression<T> x => new TypedConstantExpressionBuilder<T>(x),
            //TypedConstantResultExpression<T> x => new TypedConstantResultExpressionBuilder<T>(x),
            TypedDelegateExpression<T> x => new TypedDelegateExpressionBuilder<T>(x),
            //TypedDelegateResultExpression<T> x => new TypedDelegateResultExpressionBuilder<T>(x),
            Expression e => (ITypedExpressionBuilder<T>)e.ToBuilder(),
            _ => throw new NotSupportedException("Typed expression should be inherited from Expression")
        };
}

internal static class StringExtensions
{
    /// <summary>
    /// Gets generic type from a type string in this format: TypeName&lt;arguments&gt;
    /// </summary>
    /// <param name="instance"></param>
    /// <returns>TypeName</returns>
    internal static string WithoutGenerics(this string instance)
    {
        var index = instance.IndexOf('<', StringComparison.Ordinal);
        return index == -1
            ? instance
            : instance.Substring(0, index);
    }

    /// <summary>
    /// Gets generic type argument from a type string in this format: TypeName&lt;arguments&gt;
    /// </summary>
    /// <param name="value">Full typename, i.e. TypeName&lt;arguments&gt;</param>
    /// <returns>arguments</returns>
    internal static string GetGenericArguments(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var open = value.IndexOf('<', StringComparison.Ordinal);
        if (open == -1)
        {
            return string.Empty;
        }

        var close = value.LastIndexOf('>');

        if (close == -1)
        {
            return string.Empty;
        }

        return value.Substring(open + 1, close - open - 1);
    }

    internal static Result<Type> GetGenericTypeResult(this string functionName)
    {
        var typeName = functionName.GetGenericArguments();
        if (string.IsNullOrEmpty(typeName))
        {
            return Result.Invalid<Type>("No type defined");
        }

        var type = Type.GetType(typeName);

        return type is not null
            ? Result.Success(type)
            : Result.Invalid<Type>($"Unknown type: {typeName}");
    }
}

public static class FunctionParseResultExtensions
{
    public static ITypedExpression<T> GetArgumentValueExpression<T>(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<T>(argumentName, functionParseResult.GetArgumentValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<T> GetArgumentValueExpression<T>(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, T? defaultValue)
        => ProcessArgumentResult(argumentName, functionParseResult.GetArgumentValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue), true, defaultValue);

    public static ITypedExpression<int> GetArgumentInt32ValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<int>(argumentName, functionParseResult.GetArgumentInt32ValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<int> GetArgumentInt32ValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, int defaultValue)
        => ProcessArgumentResult<int>(argumentName, functionParseResult.GetArgumentInt32ValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static ITypedExpression<long> GetArgumentInt64ValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<long>(argumentName, functionParseResult.GetArgumentInt64ValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<long> GetArgumentInt64ValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, long defaultValue)
        => ProcessArgumentResult<long>(argumentName, functionParseResult.GetArgumentInt64ValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static ITypedExpression<decimal> GetArgumentDecimalValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<decimal>(argumentName, functionParseResult.GetArgumentDecimalValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<decimal> GetArgumentDecimalValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, decimal defaultValue)
        => ProcessArgumentResult<decimal>(argumentName, functionParseResult.GetArgumentDecimalValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static ITypedExpression<bool> GetArgumentBooleanValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<bool>(argumentName, functionParseResult.GetArgumentBooleanValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<bool> GetArgumentBooleanValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, bool defaultValue)
        => ProcessArgumentResult<bool>(argumentName, functionParseResult.GetArgumentBooleanValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static ITypedExpression<DateTime> GetArgumentDateTimeValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<DateTime>(argumentName, functionParseResult.GetArgumentDateTimeValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<DateTime> GetArgumentDateTimeValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, DateTime defaultValue)
        => ProcessArgumentResult<DateTime>(argumentName, functionParseResult.GetArgumentDateTimeValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static ITypedExpression<string> GetArgumentStringValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => ProcessArgumentResult<string>(argumentName, functionParseResult.GetArgumentStringValueResult(index, argumentName, formatProvider, context, evaluator, parser));

    public static ITypedExpression<string> GetArgumentStringValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, string defaultValue)
        => ProcessArgumentResult<string>(argumentName, functionParseResult.GetArgumentStringValueResult(index, argumentName, formatProvider, context, evaluator, parser, defaultValue));

    public static Result<T> GetArgumentExpressionResult<T>(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
        => GetArgumentValueExpression<T>(functionParseResult, index, argumentName, evaluator, parser, formatProvider, context).EvaluateTyped(context);

    public static Result<T> GetArgumentExpressionResult<T>(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context, T? defaultValue)
        => GetArgumentValueExpression(functionParseResult, index, argumentName, evaluator, parser, formatProvider, context, defaultValue).EvaluateTyped(context);

    public static ITypedExpression<IEnumerable> GetTypedExpressionsArgumentValueExpression(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
    {
        var expressions = GetArgumentValueExpression<IEnumerable>(functionParseResult, index, argumentName, evaluator, parser, formatProvider, context).EvaluateTyped(context);

        return new TypedConstantExpression<IEnumerable>(expressions.IsSuccessful()
            ? expressions.Value!.OfType<object>().Select(x => new ConstantExpression(x))
            : new Expression[] { new ConstantResultExpression(expressions) });
    }

    public static IEnumerable<Expression> GetExpressionsArgumentValueResult(this FunctionCall functionParseResult, int index, string argumentName, IFunctionEvaluator evaluator, IExpressionEvaluator parser, IFormatProvider formatProvider, object? context)
    {
        var expressions = GetArgumentValueExpression<IEnumerable>(functionParseResult, index, argumentName, evaluator, parser, formatProvider, context).EvaluateTyped(context);

        return expressions.IsSuccessful()
            ? expressions.Value!.OfType<object>().Select(x => new ConstantExpression(x))
            : new Expression[] { new ConstantResultExpression(expressions) };
    }

    private static TypedConstantResultExpression<T> ProcessArgumentResult<T>(string argumentName, Result argumentValueResult, bool useDefaultValue = false, T? defaultValue = default)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return new TypedConstantResultExpression<T>(Result.FromExistingResult<T>(argumentValueResult));
        }

        var value = argumentValueResult.GetValue();
        if (value is T t)
        {
            return new TypedConstantResultExpression<T>(Result.Success(t));
        }

        if (value is null && useDefaultValue)
        {
            return new TypedConstantResultExpression<T>(Result.Success(defaultValue!));
        }

        if (typeof(T).IsEnum && value is string s)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return new TypedConstantResultExpression<T>(Result.Success((T)Enum.Parse(typeof(T), s)));
            }
            catch (Exception ex)
            {
                return new TypedConstantResultExpression<T>(Result.Invalid<T>($"{argumentName} value [{s}] could not be converted to {typeof(T).FullName}. Error message: {ex.Message}"));
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        return new TypedConstantResultExpression<T>(Result.Invalid<T>($"{argumentName} is not of type {typeof(T).FullName}"));
    }
}
