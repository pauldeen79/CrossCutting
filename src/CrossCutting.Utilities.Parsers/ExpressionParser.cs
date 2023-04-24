namespace CrossCutting.Utilities.Parsers;

public class ExpressionParser : IExpressionParser
{
    private readonly IEnumerable<IExpressionParserProcessor> _processors;

    public ExpressionParser(IEnumerable<IExpressionParserProcessor> processors)
    {
        _processors = processors;
    }

    public Result<object?> Parse(string value, IFormatProvider formatProvider)
    {
        foreach (var processor in _processors.OrderBy(x => x.Order))
        {
            var result = processor.Parse(value, formatProvider);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        // In other cases, it's unknown
        return Result<object?>.Invalid($"Unknown expression type found in fragment: {value}");
    }
}
