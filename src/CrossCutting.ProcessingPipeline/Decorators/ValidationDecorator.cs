namespace CrossCutting.ProcessingPipeline.Decorators;

public class ValidationDecorator<TRequest> : IPipelineComponent<TRequest>
{
    private readonly IPipelineComponent<TRequest> _decoratee;

    public ValidationDecorator(IPipelineComponent<TRequest> decoratee)
    {
        ArgumentGuard.IsNotNull(decoratee, nameof(decoratee));
        
        _decoratee = decoratee;
    }

    public async Task<Result> ProcessAsync(PipelineContext<TRequest> context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        ICollection<ValidationResult> validationResults = new List<ValidationResult>();
        if (!context.Request!.TryValidate(validationResults))
        {
            return Result.Invalid(validationResults.Select(x => new ValidationError(x.ErrorMessage.ToStringWithDefault(), x.MemberNames)));
        }

        return await _decoratee.ProcessAsync(context, token).ConfigureAwait(false);
    }
}
