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

        if (!context.FindAllOccurencedNotWithinQuotes(OperatorSigns, context.Settings.StringComparison))
        {
            return Result.Continue<object?>();
        }

        var tokensResult = _tokenizer.Tokenize(context.Expression);
        if (!tokensResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(tokensResult);
        }

        var parseResult = _parser.Parse(tokensResult.GetValueOrThrow());
        if (!parseResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(parseResult);
        }

        return parseResult.GetValueOrThrow().Evaluate(context);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression);

        var tokensResult = _tokenizer.Tokenize(context.Expression);
        if (!tokensResult.IsSuccessful())
        {
            return result
                .WithErrorMessage(tokensResult.ErrorMessage)
                .WithStatus(tokensResult.Status)
                .AddValidationErrors(tokensResult.ValidationErrors);
        }

        var parseResult = _parser.Parse(tokensResult.GetValueOrThrow());
        if (!parseResult.IsSuccessful())
        {
            return result
                .WithErrorMessage(parseResult.ErrorMessage)
                .WithStatus(parseResult.Status)
                .AddValidationErrors(parseResult.ValidationErrors);
        }

        return parseResult.GetValueOrThrow().Parse(context);
    }
}
