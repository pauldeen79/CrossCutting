namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record ExpressionParseResult
{
    public Result ToResult()
        => Status.ToResult(ErrorMessage, ValidationErrors);

    public bool IsSuccessful()
        => Status.IsSuccessful();
}
