namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase
{
    protected IDictionary<Type, object?> Mocks { get; }
    protected IExpressionEvaluator Evaluator { get; }
    protected IExpressionComponent Expression => Mocks.GetOrCreate<IExpressionComponent>(ClassFactory);
    protected IMemberResolver MemberResolver => Mocks.GetOrCreate<IMemberResolver>(ClassFactory);
    protected IMemberDescriptorProvider MemberDescriptorProvider => Mocks.GetOrCreate<IMemberDescriptorProvider>(ClassFactory);
    protected IDateTimeProvider DateTimeProvider => Mocks.GetOrCreate<IDateTimeProvider>(ClassFactory);
    protected DateTime CurrentDateTime { get; }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        object? state,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
    {
        IReadOnlyDictionary<string, Task<Result<object?>>>? dict = null;
        if (state is not null)
        {
            dict = new Dictionary<string, Task<Result<object?>>>
            {
                { "state", Task.FromResult(Result.Success<object?>(state)) }
            };
        }

        return CreateContext(expression, dict, currentRecursionLevel, parentContext, evaluator, settings);
    }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        IReadOnlyDictionary<string, Task<Result<object?>>>? state = null,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
            => new ExpressionEvaluatorContext(expression, settings ?? new ExpressionEvaluatorSettingsBuilder(), evaluator ?? Evaluator, state, currentRecursionLevel, parentContext);

    protected DotExpressionComponentState CreateDotExpressionComponentState(string expression, object? left, string right)
        => new DotExpressionComponentState(CreateContext(expression), new FunctionParser(), Result.Continue<object?>(), string.Empty, null)
        {
            Value = left!,
            Part = right
        };

    protected TestBase()
    {
        Mocks = new Dictionary<Type, object?>
        {
            { typeof(IExpressionEvaluator), typeof(ExpressionEvaluator) },
            { typeof(IExpressionTokenizer), typeof(ExpressionTokenizer) },
            { typeof(IExpressionParser), typeof(ExpressionParser) },
            { typeof(IFunctionParser), typeof(FunctionParser) },
        };

        Evaluator = new ExpressionEvaluatorMock(Expression);

        // Note that you have to setup EvaluateAsync and ValidateAsync on Expression yourself
        // Note that you have to setup ResolveAsync om MemberResolver yourself
        // Note that you have to setup GetAll on MemberDescriptorProvider yourself

        // Freeze DateTime.Now to a predicatable value
        CurrentDateTime = new DateTime(2025, 2, 1, 5, 30, 0, DateTimeKind.Utc);
        DateTimeProvider
            .GetCurrentDateTime()
            .Returns(CurrentDateTime);
    }

    // Class factory for NSubstitute, see Readme.md
    protected object? ClassFactory(Type t)
        => t.CreateInstance(parameterType => Substitute.For([parameterType], []), Mocks, null, null);

    // Test stub for expression evaluation, that supports strings, integers, long integers, decimals, booleans and DeteTimes (by using TryParse), as well as the context and null keywords
    internal sealed class ExpressionEvaluatorMock : IExpressionEvaluator
    {
        private readonly IExpressionComponent _expressionComponent;

        internal ExpressionEvaluatorMock(IExpressionComponent expressionComponent)
        {
            _expressionComponent = expressionComponent;

        }
        public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        {
            if (context.Expression == "null")
            {
                return Result.Success(default(object?));
            }

            var success = context.State.TryGetValue(context.Expression, out var dlg);
            if (success && dlg is not null)
            {
                return await dlg.ConfigureAwait(false);
            }

            if (context.Expression == "state.Length")
            {
                return Result.Success<object?>((context.State?.ToString() ?? string.Empty).Length);
            }

            if (context.Expression == "error")
            {
                return Result.Error<object?>("Kaboom");
            }

            if (context.Expression == "recursiveplaceholder")
            {
                return Result.Success<object?>("{recurse}");
            }

            if (context.Expression == "recurse")
            {
                return Result.Success<object?>("recursive value");
            }

            if (context.Expression.StartsWith('"') && context.Expression.StartsWith('"'))
            {
                return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
            }

            if (int.TryParse(context.Expression, context.Settings.FormatProvider, out int number))
            {
                return Result.Success<object?>(number);
            }

            if (context.Expression.EndsWith('L') && long.TryParse(context.Expression[..^1], context.Settings.FormatProvider, out long longNumber))
            {
                return Result.Success<object?>(longNumber);
            }

            if (context.Expression.EndsWith('M') && decimal.TryParse(context.Expression[..^1], context.Settings.FormatProvider, out decimal decimalNumber))
            {
                return Result.Success<object?>(decimalNumber);
            }

            if (context.Expression.StartsWith("System."))
            {
                return Result.Success<object?>(Type.GetType(context.Expression));
            }

            if (bool.TryParse(context.Expression, out bool boolean))
            {
                return Result.Success<object?>(boolean);
            }

            if (context.Expression.StartsWith('"') && context.Expression.EndsWith('"'))
            {
                return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
            }

            if (DateTime.TryParse(context.Expression, context.Settings.FormatProvider, out DateTime dateTime))
            {
                return Result.Success<object?>(dateTime);
            }

            var result = await _expressionComponent.ParseAsync(context, token).ConfigureAwait(false);
            if (result?.Status == ResultStatus.Ok)
            {
                return await _expressionComponent.EvaluateAsync(context, token).ConfigureAwait(false);
            }

            if (context.Expression == "MyNestedFunction()")
            {
                return Result.Success<object?>("Evaluated result");
            }

            if (context.Expression == "exception")
            {
                throw new InvalidOperationException("Kaboom");
            }

            return Result.NotSupported<object?>($"Unsupported expression: {context.Expression}");
        }

        public Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<T>> EvaluateTypedAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
            => (await EvaluateAsync(context, token).ConfigureAwait(false)).TryCastAllowNull<T>();

        public Task<Result<T>> EvaluateTypedCallbackAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.Run<ExpressionParseResult>(() => context.Expression switch
            {
                "error" or "666" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Error).WithErrorMessage("Kaboom"),
                "-1" or "1" or "2" or "123" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(int)),
                "string" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(string)),
                "unknown" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok),
                "object" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(object)),
                "state" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(context.State?.FirstOrDefault().Value?.Result.Value?.GetType()),
                _ => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok)
            });

        public Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Testing.CreateInstance<T>(ClassFactory, Mocks, p =>
    {
        // Use real implementations for internal types
        if (p.ParameterType == typeof(IEnumerable<IDotExpressionComponent>))
        {
            return new IDotExpressionComponent[]
            {
                new ReflectionMethodDotExpressionComponent(),
                new ReflectionPropertyDotExpressionComponent()
            };
        }

        return null;
    })!;
}
