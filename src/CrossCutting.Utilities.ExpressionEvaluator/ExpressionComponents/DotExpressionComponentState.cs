namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponentState
{
    public ExpressionEvaluatorContext Context { get; }
    public StringBuilder CurrentExpression { get; }
    public string Part { get; set; }
    public object Value { get; set; }
    public Type? ResultType { get; set; }

    public DotExpressionComponentState(ExpressionEvaluatorContext context, string firstPart)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(firstPart, nameof(firstPart));

        Context = context;
        CurrentExpression = new StringBuilder(firstPart);
        Part = string.Empty;
        Value = default!;
    }

    public void AppendPart()
        => CurrentExpression.Append('.').Append(Part);
}
