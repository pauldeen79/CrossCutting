namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record ExpressionParseResult
{
    public Result<Type> ToResult()
        => Status.ToTypedResult(ResultType, ErrorMessage, ValidationErrors);

    public bool IsSuccessful()
        => Status.IsSuccessful();

    public static implicit operator Result<Type>(ExpressionParseResult instance)
    {
        instance = ArgumentGuard.IsNotNull(instance, nameof(instance));

        return instance.ToResult();
    }
}
