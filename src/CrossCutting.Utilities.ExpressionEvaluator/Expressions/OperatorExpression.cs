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

        return new ResultDictionaryBuilder()
            .Add("Tokenize", () => _tokenizer.Tokenize(context.Expression))
            .Add("Parse", results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>("Tokenize")))
            .Add("Evaluate", results => results.GetValue<IOperator>("Parse").Evaluate(context))
            .Build()
            .Aggregate<object?>();
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

        var results = new ResultDictionaryBuilder()
            .Add("Tokenize", () => _tokenizer.Tokenize(context.Expression))
            .Add("Parse", results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>("Tokenize")))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return result.FillFromResult(error);
        }

        return results.GetValue<IOperator>("Parse").Parse(context);
    }
}
