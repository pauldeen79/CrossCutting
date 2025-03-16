namespace CrossCutting.Utilities.Parsers;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public IExpressionEvaluator Evaluator { get; }

    public ExpressionEvaluatorContext(string expression, ExpressionEvaluatorSettings settings, object? context, IExpressionEvaluator evaluator)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression;
        Settings = settings;
        Context = context;
        Evaluator = evaluator;
    }
}
