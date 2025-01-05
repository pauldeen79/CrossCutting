namespace CrossCutting.Utilities.Parsers;

public class FunctionCallContext
{
    public FunctionCallContext(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        FunctionCall = functionCall ?? throw new ArgumentNullException(nameof(functionCall));
        FunctionEvaluator = functionEvaluator ?? throw new ArgumentNullException(nameof(functionEvaluator));
        ExpressionEvaluator = expressionEvaluator ?? throw new ArgumentNullException(nameof(expressionEvaluator));
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
        Context = context;
    }

    public FunctionCall FunctionCall { get; }
    public IFunctionEvaluator FunctionEvaluator { get; }
    public IExpressionEvaluator ExpressionEvaluator { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
}
