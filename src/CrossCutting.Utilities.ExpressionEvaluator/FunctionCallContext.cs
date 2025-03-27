namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionCallContext
{
    public FunctionCallContext(FunctionCall functionCall, ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));
        ArgumentGuard.IsNotNull(context, nameof(context));

        FunctionCall = functionCall;
        Context = context;
    }

    public FunctionCall FunctionCall { get; }
    public ExpressionEvaluatorContext Context { get; }
}
