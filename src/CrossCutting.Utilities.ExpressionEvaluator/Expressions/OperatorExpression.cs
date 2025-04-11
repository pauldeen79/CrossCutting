namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class OperatorExpression : IExpression
{
    private static readonly string[] OperatorSigns = ["+", "-", "*", "/", "(", ")", "==", "!", "<", ">", "&&", "||"];

    public int Order => 30;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (!context.FindAllOccurencedNotWithinQuotes(OperatorSigns, context.Settings.StringComparison))
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
        var parseResult = parser.Parse();
        if (!parseResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(parseResult);
        }

        return parseResult.Value!.Evaluate(context, value => context.Evaluate(value));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression);

        var tokenizer = new ExpressionTokenizer.ExpressionTokenizer(context.Expression);
        var tokensResult = tokenizer.Tokenize();
        if (!tokensResult.IsSuccessful())
        {
            return result
                .WithErrorMessage(tokensResult.ErrorMessage)
                .WithStatus(tokensResult.Status)
                .AddValidationErrors(tokensResult.ValidationErrors);
        }

        var parser = new ExpressionParser.ExpressionParser(tokensResult.Value!);
        var parseResult = parser.Parse();
        if (!parseResult.IsSuccessful())
        {
            return result
                .WithErrorMessage(parseResult.ErrorMessage)
                .WithStatus(parseResult.Status)
                .AddValidationErrors(parseResult.ValidationErrors);
        }

        //TODO: Add ExpressionType to IOperatorExpression
        return result/*.WithExpressionType(parseResult.Value.ExpressionType)*/;
    }
}
