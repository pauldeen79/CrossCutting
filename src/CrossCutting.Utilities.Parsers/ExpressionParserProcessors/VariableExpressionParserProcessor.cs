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
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        if (value.StartsWith("$"))
        {
            return _variableProcessor.Process(value.Substring(1), context);
        }

        return Result.Continue<object?>();
    }
}
