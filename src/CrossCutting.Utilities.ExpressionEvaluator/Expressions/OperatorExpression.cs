namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class OperatorExpression : IExpression
{
    private static readonly string[] OperatorSigns = ["+", "-", "*", "/", "(", ")", "==", "!", "<", ">", "&&", "||"];
    private readonly IOperatorExpressionTokenizer _tokenizer;
    private readonly IOperatorExpressionParser _parser;

    public OperatorExpression(IOperatorExpressionTokenizer tokenizer, IOperatorExpressionParser parser)
    {
        ArgumentGuard.IsNotNull(tokenizer, nameof(tokenizer));
        ArgumentGuard.IsNotNull(parser, nameof(parser));
        
        _tokenizer = tokenizer;
        _parser = parser;
    }
    public int Order => 30;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (!context.HasAnyOccurenceNotWithinQuotes(OperatorSigns, context.Settings.StringComparison))
        {
            return Result.Continue<object?>();
        }

        var results = new ResultDictionaryBuilder()
            .Add("Tokenize", () => _tokenizer.Tokenize(context.Expression))
            .Add("Parse", results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>("Tokenize")))
            .Add("Evaluate", results => results.GetValue<IOperator>("Parse").Evaluate(context))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return Result.FromExistingResult<object?>(results["Evaluate"]);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression);

        if (!context.HasAnyOccurenceNotWithinQuotes(OperatorSigns, context.Settings.StringComparison))
        {
            return result.WithStatus(ResultStatus.Continue);
        }
        
        var tokensResult = _tokenizer.Tokenize(context.Expression);
        if (!tokensResult.IsSuccessful())
        {
            return result.FillFromResult(tokensResult);
        }

        var parseResult = _parser.Parse(tokensResult.GetValueOrThrow());
        if (!parseResult.IsSuccessful())
        {
            return result.FillFromResult(parseResult);
        }

        return parseResult.GetValueOrThrow().Parse(context);
    }
}
