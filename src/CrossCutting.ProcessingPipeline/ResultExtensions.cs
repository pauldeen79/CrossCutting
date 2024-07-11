namespace CrossCutting.ProcessingPipeline;

public static class ResultExtensions
{
    public static Result<TResult> ProcessResult<TResult>(this Result instance, object responseBuilder, Func<TResult> resultValueDelegate)
    {
        resultValueDelegate = resultValueDelegate.IsNotNull(nameof(resultValueDelegate));

        if (!instance.IsSuccessful())
        {
            return Result.FromExistingResult<TResult>(instance);
        }

        var validationResults = new List<ValidationResult>();
        var success = responseBuilder.TryValidate(validationResults);
        if (!success)
        {
            return Result.Invalid<TResult>("Pipeline response is not valid", validationResults.Select(x => new ValidationError(x.ErrorMessage ?? string.Empty, x.MemberNames)));
        }

        return Result.FromExistingResult(instance, resultValueDelegate.Invoke());
    }
}
