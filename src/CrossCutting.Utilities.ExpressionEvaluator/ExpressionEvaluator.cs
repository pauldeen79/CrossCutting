namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpressionTokenizer _tokenizer;
    private readonly IExpressionParser _parser;
    private readonly IExpressionComponent[] _components;

    public ExpressionEvaluator(IExpressionTokenizer tokenizer, IExpressionParser parser, IEnumerable<IExpressionComponent> components)
    {
        ArgumentGuard.IsNotNull(tokenizer, nameof(tokenizer));
        ArgumentGuard.IsNotNull(parser, nameof(parser));
        ArgumentGuard.IsNotNull(components, nameof(components));

        _tokenizer = tokenizer;
        _parser = parser;
        _components = components.OrderBy(x => x.Order).ToArray();
    }

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        var results = new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context).EnsureNotNull("Tokenize returned null"))
            .Add(nameof(IExpressionParser.Parse), results => _parser.Parse(context, results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))).EnsureNotNull("Parse returned null"))
            .Build();
            
        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return await results.GetValue<IExpression>(nameof(IExpressionParser.Parse)).EvaluateAsync(token).ConfigureAwait(false);
    }

    public async Task<Result<T>> EvaluateTypedAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        var results = new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context).EnsureNotNull("Tokenize returned null"))
            .Add(nameof(IExpressionParser.Parse), results => _parser.Parse(context, results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))).EnsureNotNull("Parse returned null"))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<T>(error);
        }

        var expression = results.GetValue<IExpression>(nameof(IExpressionParser.Parse));

        return expression is IExpression<T> typedExpression
            ? await typedExpression.EvaluateTypedAsync(token).ConfigureAwait(false)
            : (await expression.EvaluateAsync(token).ConfigureAwait(false)).TryCastAllowNull<T>();
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        var result = new ExpressionParseResultBuilder().WithSourceExpression(context.Expression);

        var results = new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context).EnsureNotNull("Tokenize returned null"))
            .Add(nameof(ParseAsync), results => _parser.Parse(context, results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))).EnsureNotNull("Parse returned null"))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return result.FillFromResult(error);
        }

        return await results.GetValue<IExpression>(nameof(ParseAsync)).ParseAsync(token).ConfigureAwait(false);
    }

    public Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<object?>()
            .OnSuccess(async () =>
            {
                foreach (var component in _components)
                {
                    Result<object?> result;

#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        result = (await component.EvaluateAsync(context, token).ConfigureAwait(false))
                            .EnsureNotNull("EvaluateAsync returned null");
                    }
                    catch (Exception ex)
                    {
                        result = Result.Error<object?>(ex, "Exception occured");
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    if (result.Status != ResultStatus.Continue)
                    {
                        return result;
                    }
                }

                return Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}");
            });
    }

    public Task<Result<T>> EvaluateTypedCallbackAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<T>()
            .OnSuccess(async () =>
            {
                foreach (var component in _components)
                {
                    Result<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        result = component is IExpressionComponent<T> typedComponent
                            ? (await typedComponent.EvaluateTypedAsync(context, token).ConfigureAwait(false))
                                .EnsureNotNull("EvaluateAsync returned null")
                            : (await component.EvaluateAsync(context, token).ConfigureAwait(false))
                                .EnsureNotNull("EvaluateAsync returned null")
                                .TryCastAllowNull<T>();
                    }
                    catch (Exception ex)
                    {
                        result = Result.Error<T>(ex, "Exception occured");
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    if (result.Status != ResultStatus.Continue)
                    {
                        return result;
                    }
                }

                return Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}");
            });
    }

    public async Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<ExpressionParseResult>();
        if (!validationResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder().FillFromResult(validationResult);
        }

        foreach (var component in _components)
        {
            var result = (await component.ParseAsync(context, token).ConfigureAwait(false))
                ?? new ExpressionParseResultBuilder()
                    .WithSourceExpression(context.Expression)
                    .WithStatus(ResultStatus.Error)
                    .WithErrorMessage($"ParseAsync returned null for component type {component.GetType().FullName}");
            
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Invalid)
            .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}");
    }
}
