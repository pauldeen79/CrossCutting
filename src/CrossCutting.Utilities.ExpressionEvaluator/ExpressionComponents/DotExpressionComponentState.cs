namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

internal sealed class DotExpressionComponentState
{
    public ExpressionEvaluatorContext Context { get; }
    public IFunctionParser FunctionParser { get; }
    public StringBuilder CurrentExpression { get; }
    public string Part { get; set; }
    public object Value { get; set; }
    public Type? ResultType { get; set; }

    public DotExpressionComponentState(ExpressionEvaluatorContext context, IFunctionParser functionParser, string firstPart)
    {
        Context = context;
        FunctionParser = functionParser;
        CurrentExpression = new StringBuilder(firstPart);
        Part = string.Empty;
        Value = default!;
    }

    public void AppendPart()
        => CurrentExpression.Append('.').Append(Part);
}
