namespace CrossCutting.Utilities.QueryEvaluator;

public partial record Expression
{
    public abstract Result<object?> Evaluate(object? context);
}
