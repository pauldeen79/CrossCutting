namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpression[] _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<object?>();
        if (!validationResult.IsSuccessful())
        {
            return validationResult;
        }

        // First try simple expression
        var expressionResult = _expressions
            .Select(x => x.Evaluate(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        if (expressionResult is not null)
        {
            return expressionResult;
        }

        var tokenizer = new ExpressionTokenizer.ExpressionTokenizer(context.Expression);
        var tokensResult = tokenizer.Tokenize();
        if (!tokensResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(tokensResult);
        }
        var parser = new ExpressionParser.ExpressionParser(tokensResult.Value!);
        var exprResult = parser.Parse();
        if (!exprResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(exprResult);
        }

        return exprResult.Value!.Evaluate(context, value => DoEvaluate(context.CreateChildContext(value)));
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<T>();
        if (!validationResult.IsSuccessful())
        {
            return validationResult;
        }

        return DoEvaluateTyped<T>(context);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<ExpressionParseResult>();
        if (!validationResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(validationResult.Status)
                .WithErrorMessage(validationResult.ErrorMessage);
        }

        return DoParse(context);
    }

    private Result<object?> DoEvaluate(ExpressionEvaluatorContext context)
        => _expressions
            .Select(x => x.Evaluate(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}");

    private Result<T> DoEvaluateTyped<T>(ExpressionEvaluatorContext context)
        => _expressions
            .Select(x => x is IExpression<T> typedExpression
                ? typedExpression.EvaluateTyped(context)
                : x.Evaluate(context).TryCastAllowNull<T>())
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}");

    private ExpressionParseResult DoParse(ExpressionEvaluatorContext context)
    {
        var expressionParseResult = _expressions
            .Select(x => x.Parse(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        return expressionParseResult is null
            ? new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}")
            : expressionParseResult;
    }
}
