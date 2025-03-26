namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionParseResultBuilder
{
    public void AddPartResult(ExpressionParseResult itemResult, string partName)
    {
        itemResult = ArgumentGuard.IsNotNull(itemResult, nameof(itemResult));
        partName = ArgumentGuard.IsNotNullOrEmpty(partName, nameof(partName));

        AddPartResults(new ExpressionParsePartResultBuilder()
            .WithPartName(partName)
            .WithStatus(itemResult.Status)
            .AddValidationErrors(itemResult.ValidationErrors)
            .WithErrorMessage(itemResult.ErrorMessage)
            .WithExpressionType(itemResult.ExpressionType)
            .WithResultType(itemResult.ResultType)
            .AddPartResults(itemResult.PartResults.Select(x => x.ToBuilder()))
            .WithSourceExpression(itemResult.SourceExpression)
            );
    }

    public ExpressionParseResultBuilder DetectStatusFromPartResults()
    {
        var error = PartResults.FirstOrDefault(x => !x.Status.IsSuccessful());
        
        return error is not null
            ? this.WithStatus(error.Status).WithErrorMessage("Parsing of the expression failed, see inner results for details")
            : this.WithStatus(ResultStatus.Ok);
    }
}
