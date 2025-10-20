namespace CrossCutting.ProcessingPipeline.Decorators;

public class ExceptionDecorator<TRequest> : IPipelineComponent<TRequest>
{
    private readonly IPipelineComponent<TRequest> _decoratee;

    public ExceptionDecorator(IPipelineComponent<TRequest> decoratee)
    {
        ArgumentGuard.IsNotNull(decoratee, nameof(decoratee));

        _decoratee = decoratee;
    }

    public async Task<Result> ProcessAsync(PipelineContext<TRequest> context, CancellationToken token)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return await _decoratee.ProcessAsync(context, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Error(ex, "Error occured, see Exception for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
