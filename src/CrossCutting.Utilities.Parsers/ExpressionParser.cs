namespace CrossCutting.Utilities.Parsers;

public class ExpressionParser : IExpressionParser
{
    private readonly IEnumerable<IExpressionParserProcessor> _processors;

    public ExpressionParser(IEnumerable<IExpressionParserProcessor> processors)
    {
        _processors = processors;
    }

    public Result<object?> Parse(string value, IFormatProvider formatProvider)
        => _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Parse(value, formatProvider))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result<object?>.Invalid($"Unknown expression type found in fragment: {value}");
}
