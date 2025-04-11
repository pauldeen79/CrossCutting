namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionParseResultBuilder
{
    public ExpressionParseResultBuilder AddPartResult(ExpressionParseResult itemResult, string partName)
    {
        itemResult = ArgumentGuard.IsNotNull(itemResult, nameof(itemResult));
        partName = ArgumentGuard.IsNotNullOrEmpty(partName, nameof(partName));

        return AddPartResults(new ExpressionParsePartResultBuilder()
            .WithPartName(partName)
            .FillFromResult(itemResult.ToResult())
            .WithExpressionType(itemResult.ExpressionType)
            .WithResultType(itemResult.ResultType)
            .AddPartResults(itemResult.PartResults.Select(x => x.ToBuilder()))
            .WithSourceExpression(itemResult.SourceExpression)
            );
    }

    public ExpressionParseResultBuilder SetStatusFromPartResults()
    {
        var error = PartResults.FirstOrDefault(x => !x.Status.IsSuccessful());
        
        return error is not null
            ? this.WithStatus(error.Status).WithErrorMessage("Parsing of the expression failed, see inner results for details")
            : this.WithStatus(ResultStatus.Ok);
    }

    public ExpressionParseResultBuilder FillFromResult(Result result)
    {
        result = ArgumentGuard.IsNotNull(result, nameof(result));

        return this
            .WithErrorMessage(result.ErrorMessage)
            .WithStatus(result.Status)
            .AddValidationErrors(result.ValidationErrors);
    }
}
