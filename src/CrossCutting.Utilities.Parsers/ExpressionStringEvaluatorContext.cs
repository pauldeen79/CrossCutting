namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringEvaluatorContext
{
    public string Input { get; }
    public ExpressionStringEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public IExpressionStringEvaluator Parser { get; }
    public IFormattableStringParser? FormattableStringParser { get; }

    public ExpressionStringEvaluatorContext(
        string input,
        ExpressionStringEvaluatorSettings settings,
        object? context,
        IExpressionStringEvaluator parser,
        IFormattableStringParser? formattableStringParser)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(parser, nameof(parser));

        Input = input;
        Settings = settings;
        Context = context;
        Parser = parser;
        FormattableStringParser = formattableStringParser;
    }
}
