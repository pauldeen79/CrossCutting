namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionParsePartResultBuilder
{
    public ExpressionParsePartResultBuilder FillFromResult(Result result)
    {
        result = ArgumentGuard.IsNotNull(result, nameof(result));

        return this
            .WithErrorMessage(result.ErrorMessage)
            .WithStatus(result.Status)
            .AddValidationErrors(result.ValidationErrors);
    }
}
