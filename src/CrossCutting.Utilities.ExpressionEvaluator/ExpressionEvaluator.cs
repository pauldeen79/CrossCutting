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

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        return new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(IExpressionParser.Parse), results => _parser.Parse(results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))))
            .Add(nameof(Evaluate), results => results.GetValue<IExpression>(nameof(Parse)).Evaluate(context))
            .Build()
            .Aggregate<object?>();
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        return new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(IExpressionParser.Parse), results => _parser.Parse(results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))))
            .Add(nameof(Evaluate), results => results.GetValue<IExpression>(nameof(Parse)).EvaluateTyped<T>(context))
            .Build()
            .Aggregate<T>();
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        var result = new ExpressionParseResultBuilder().WithSourceExpression(context.Expression);

        var results = new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(Parse), results => _parser.Parse(results.GetValue<List<ExpressionToken>>(nameof(IExpressionTokenizer.Tokenize))))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return result.FillFromResult(error);
        }

        return results.GetValue<IExpression>(nameof(Parse)).Parse(context);
    }

    public Result<object?> EvaluateCallback(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<object?>()
            .OnSuccess(() => _components
                .Select(x => x.Evaluate(context))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}"));
    }

    public Result<T> EvaluateTypedCallback<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<T>()
            .OnSuccess(() => _components
                .Select(x => x.Evaluate(context).TryCastAllowNull<T>())
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}"));
    }

    public ExpressionParseResult ParseCallback(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<ExpressionParseResult>();
        if (!validationResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder().FillFromResult(validationResult);
        }

        var expression = _components
            .Select(x => x.Parse(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        return expression is null
            ? new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}")
            : expression;
    }
}
