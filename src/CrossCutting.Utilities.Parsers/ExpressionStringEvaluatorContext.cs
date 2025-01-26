namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringEvaluatorContext
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
    public IExpressionStringEvaluator Parser { get; }
    public IFormattableStringParser? FormattableStringParser { get; }

    public ExpressionStringEvaluatorContext(
        string input,
        IFormatProvider formatProvider,
        object? context,
        IExpressionStringEvaluator parser,
        IFormattableStringParser? formattableStringParser)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));
        ArgumentGuard.IsNotNull(parser, nameof(parser));

        Input = input;
        FormatProvider = formatProvider;
        Context = context;
        Parser = parser;
        FormattableStringParser = formattableStringParser;
    }
}
