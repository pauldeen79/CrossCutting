namespace CrossCutting.Utilities.Parsers;

public class ExpressionParser : IExpressionParser
{
    private readonly IEnumerable<IExpressionParserProcessor> _processors;

    public ExpressionParser(IEnumerable<IExpressionParserProcessor> processors)
    {
        ArgumentGuard.IsNotNull(processors, nameof(processors));

        _processors = processors;
    }

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        ArgumentGuard.IsNotNull(value, nameof(value));
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));

        return _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Parse(value, formatProvider, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown expression type found in fragment: {value}");
    }
}
