namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class OperatorExpression : IExpression
{
    private readonly IOperatorExpressionTokenizer _tokenizer;
    private readonly IOperatorExpressionParser _parser;

    private static readonly string[] OperatorSigns = ["+", "-", "*", "/", "(", ")", "==", "!", "<", ">", "&&", "||"];

    public const string Tokenize = nameof(Tokenize);

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
            .Add(nameof(Tokenize), () => _tokenizer.Tokenize(context.Expression))
            .Add(nameof(Parse), results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>(nameof(Tokenize))))
            .Add(nameof(Evaluate), results => results.GetValue<IOperator>(nameof(Parse)).Evaluate(context))
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
            .Add(nameof(Tokenize), () => _tokenizer.Tokenize(context.Expression))
            .Add(nameof(Parse), results => _parser.Parse(results.GetValue<List<OperatorExpressionToken>>(nameof(Tokenize))))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return result.FillFromResult(error);
        }

        return results.GetValue<IOperator>(nameof(Parse)).Parse(context);
    }
}
