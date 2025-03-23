namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public IExpressionEvaluator Evaluator { get; }
    public IEnumerable<(int StartIndex, int EndIndex)> QuoteMap { get; }

    public ExpressionEvaluatorContext(string expression, ExpressionEvaluatorSettings settings, object? context, IExpressionEvaluator evaluator)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression.Trim();
        Settings = settings;
        Context = context;
        Evaluator = evaluator;
        QuoteMap = BuildQuoteMap(Expression);
    }

    public bool IsInQuoteMap(int index)
        => QuoteMap.Any(x => x.StartIndex < index && x.EndIndex > index);

    public bool FindAllOccurencedNotWithinQuotes(char[] charactersToFind)
    {
        charactersToFind = ArgumentGuard.IsNotNull(charactersToFind, nameof(charactersToFind));

        foreach (var comparisonChar in charactersToFind)
        {
            var occurences = Expression.FindAllOccurences(comparisonChar).Where(x => !IsInQuoteMap(x)).ToArray();
            if (occurences.Length > 0)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<(int StartIndex, int EndIndex)> BuildQuoteMap(string value)
    {
        var inText = false;
        var index = -1;
        var lastQuote = -1;

        foreach (var character in value)
        {
            index++;
            if (character == '\"')
            {
                inText = !inText;
                if (inText)
                {
                    lastQuote = index;
                }
                else
                {
                    yield return new(lastQuote, index);
                }
            }
        }
    }
}
