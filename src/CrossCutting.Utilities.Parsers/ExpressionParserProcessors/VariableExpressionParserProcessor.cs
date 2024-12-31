namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class VariableExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 50;

    private readonly IVariableProcessor _variableProcessor;

    public VariableExpressionParserProcessor(IVariableProcessor variableProcessor)
    {
        ArgumentGuard.IsNotNull(variableProcessor, nameof(variableProcessor));

        _variableProcessor = variableProcessor;
    }

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
        => value?.StartsWith("$") switch
        {
            true when value.Length > 1 => _variableProcessor.Process(value.Substring(1), context),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value?.StartsWith("$") switch
        {
            true when value.Length > 1 => _variableProcessor.Validate(value.Substring(1), context),
            _ => Result.Continue()
        };
}
