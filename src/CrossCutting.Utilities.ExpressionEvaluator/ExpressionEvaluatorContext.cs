namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public IEnumerable<(int StartIndex, int EndIndex)> QuoteMap { get; }
    public int CurrentRecursionLevel { get; }
    public ExpressionEvaluatorContext? ParentContext { get; }
    private IExpressionEvaluator Evaluator { get; }

    public ExpressionEvaluatorContext(string? expression, ExpressionEvaluatorSettings settings, object? context, IExpressionEvaluator evaluator, int currentRecursionLevel = 1, ExpressionEvaluatorContext? parentContext = null)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression?.Trim() ?? string.Empty;
        Settings = settings;
        Context = context;
        Evaluator = evaluator;
        QuoteMap = BuildQuoteMap(Expression);
        CurrentRecursionLevel = currentRecursionLevel;
        ParentContext = parentContext;
    }

    public bool IsInQuoteMap(int index)
        => QuoteMap.Any(x => x.StartIndex < index && x.EndIndex > index);

    public bool FindAllOccurencedNotWithinQuotes(IEnumerable<string> stringsToFind, StringComparison stringComparison)
    {
        stringsToFind = ArgumentGuard.IsNotNull(stringsToFind, nameof(stringsToFind));

        foreach (var stringToFind in stringsToFind)
        {
            var occurences = Expression.FindAllOccurences(stringToFind, stringComparison).Where(x => !IsInQuoteMap(x)).ToArray();
            if (occurences.Length > 0)
            {
                return true;
            }
        }

        return false;
    }

    public Result<object?> Evaluate(string expression)
        => Evaluator.Evaluate(CreateChildContext(expression));

    public ExpressionParseResult Parse(string expression)
        => Evaluator.Parse(CreateChildContext(expression));

    public Result<T> Validate<T>()
    {
        if (string.IsNullOrEmpty(Expression))
        {
            return Result.Invalid<T>("Value is required");
        }

        if (CurrentRecursionLevel > Settings.MaximumRecursion)
        {
            return Result.Invalid<T>("Maximum recursion level has been reached");
        }

        return Result.NoContent<T>();
    }

    private ExpressionEvaluatorContext CreateChildContext(string expression)
        => new ExpressionEvaluatorContext(expression, Settings, Context, Evaluator, CurrentRecursionLevel + 1, this);

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
