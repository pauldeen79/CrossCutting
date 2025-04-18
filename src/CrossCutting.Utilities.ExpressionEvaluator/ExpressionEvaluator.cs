namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IOperatorExpressionTokenizer _tokenizer;
    private readonly IOperatorExpressionParser _parser;
    private readonly IExpression[] _expressions;

    public ExpressionEvaluator(IOperatorExpressionTokenizer tokenizer, IOperatorExpressionParser parser, IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(tokenizer, nameof(tokenizer));
        ArgumentGuard.IsNotNull(parser, nameof(parser));
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _tokenizer = tokenizer;
        _parser = parser;
        _expressions = expressions.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        return new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IOperatorExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(IOperatorExpressionParser.Parse), results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>(nameof(IOperatorExpressionTokenizer.Tokenize))))
            .Add(nameof(Evaluate), results => results.GetValue<IOperator>(nameof(Parse)).Evaluate(context))
            .Build()
            .Aggregate<object?>();
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        return new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IOperatorExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(IOperatorExpressionParser.Parse), results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>(nameof(IOperatorExpressionTokenizer.Tokenize))))
            .Add(nameof(Evaluate), results => results.GetValue<IOperator>(nameof(Parse)).Evaluate(context))
            .Build()
            .Aggregate<object?>()
            .TryCastAllowNull<T>();
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        context = context.FromRoot();

        var result = new ExpressionParseResultBuilder().WithSourceExpression(context.Expression);

        var results = new ResultDictionaryBuilder()
            .Add("Validate", () => context.Validate<object?>())
            .Add(nameof(IOperatorExpressionTokenizer.Tokenize), () => _tokenizer.Tokenize(context))
            .Add(nameof(Parse), results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>(nameof(IOperatorExpressionTokenizer.Tokenize))))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return result.FillFromResult(error);
        }

        return results.GetValue<IOperator>(nameof(Parse)).Parse(context);
    }

    public Result<object?> EvaluateCallback(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<object?>()
            .OnSuccess(() => _expressions
                .Select(x => x.Evaluate(context))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}"));
    }

    public Result<T> EvaluateTypedCallback<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<T>()
            .OnSuccess(() => _expressions
                .Select(x => x is IExpression<T> typedExpression
                    ? typedExpression.EvaluateTyped(context)
                    : x.Evaluate(context).TryCastAllowNull<T>())
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

        var expression = _expressions
            .Select(x => x.Parse(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        return expression is null
            ? new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}")
            : expression;
    }
}
