namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class OperatorExpression : IExpression
{
    private static readonly string[] TokenSigns = ["+", "-", "*", "/", "(", ")", "==", "!", "<", ">", "&&", "||"];

    public int Order => 30;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (!context.FindAllOccurencedNotWithinQuotes(TokenSigns, context.Settings.StringComparison))
        {
            return Result.Continue<object?>();
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

        return exprResult.Value!.Evaluate(context, value => context.Evaluate(value));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression);

        return result;
    }
}
