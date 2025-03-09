namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringEvaluatorContext
{
    public string Input { get; }
    public ExpressionStringEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public IExpressionStringEvaluator Evaluator { get; }
    public IFormattableStringParser? FormattableStringParser { get; }

    public ExpressionStringEvaluatorContext(
        string input,
        ExpressionStringEvaluatorSettings settings,
        object? context,
        IExpressionStringEvaluator evaluator,
        IFormattableStringParser? formattableStringParser)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Input = input;
        Settings = settings;
        Context = context;
        Evaluator = evaluator;
        FormattableStringParser = formattableStringParser;
    }
}
