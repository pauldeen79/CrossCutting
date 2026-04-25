namespace CrossCutting.ProcessingPipeline;

public class PipelineHandler<TCommand>(IEnumerable<IPipelineComponentInterceptor> interceptors, IEnumerable<IPipelineComponent<TCommand>> components) : ICommandHandler<TCommand>
{
    private readonly IPipelineComponentInterceptor[] _interceptors = ArgumentGuard.IsNotNull(interceptors, nameof(interceptors))
        .OrderBy(x => (x as IOrderContainer)?.Order)
        .ToArray();
    private readonly IPipelineComponent<TCommand>[] _components = ArgumentGuard.IsNotNull(components, nameof(components))
        .OrderBy(x => (x as IOrderContainer)?.Order)
        .ToArray();

    public async Task<Result> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token)
        => await Result.EnsureNotNull<TCommand>(command, nameof(command))
            .OnSuccessAsync(async () =>
            {
                var results = new List<Result>();
                foreach (var component in _components)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    var result = await DoExecuteAsync(component, command, commandService, token)
                        .ConfigureAwait(false);

                    results.Add(result);
                    if (!result.IsSuccessful())
                    {
                        break;
                    }
                }

                return Result.Aggregate
                (
                    results,
                    Result.Success(),
                    errors => Result.Error(errors, "An error occured while processing the pipeline. See the inner results for more details.")
                );
            }).ConfigureAwait(false);

    private async Task<Result> DoExecuteAsync(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Length)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return component.ExecuteAsync(command, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}

public class PipelineHandler<TCommand, TResponse>(IEnumerable<IPipelineComponentInterceptor> interceptors, IPipelineResponseGenerator responseGenerator, IEnumerable<IPipelineComponent<TCommand, TResponse>> components) : ICommandHandler<TCommand, TResponse>
{
    private readonly IPipelineComponentInterceptor[] _interceptors = ArgumentGuard.IsNotNull(interceptors, nameof(interceptors))
        .OrderBy(x => (x as IOrderContainer)?.Order)
        .ToArray();
    private readonly IPipelineResponseGenerator _responseGenerator = ArgumentGuard.IsNotNull(responseGenerator, nameof(responseGenerator));
    private readonly IPipelineComponent<TCommand, TResponse>[] _components = ArgumentGuard.IsNotNull(components, nameof(components))
        .OrderBy(x => (x as IOrderContainer)?.Order)
        .ToArray();

    public async Task<Result<TResponse>> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token)
            => await Result.Validate<TResponse>(() => command is not null, "command is required")
            .OnSuccessAsync(async () =>
            {
                var results = new List<Result>();

                return await _responseGenerator.Generate<TResponse>(command!)
                    .EnsureValue()
                    .OnSuccessAsync(async response =>
                    {
                        foreach (var component in _components)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            }

                            var result = await DoExecuteAsync(component, command, response.Value!, commandService, token)
                                .ConfigureAwait(false);

                            results.Add(result);
                            if (!result.IsSuccessful())
                            {
                                break;
                            }
                        }

                        return Result.Aggregate
                        (
                            results,
                            response.Value!,
                            errors => Result.Error<TResponse>(errors, "An error occured while processing the pipeline. See the inner results for more details.")
                        );
                    }).ConfigureAwait(false);
            }).ConfigureAwait(false);

    private async Task<Result> DoExecuteAsync(IPipelineComponent<TCommand, TResponse> component, TCommand command, TResponse response, ICommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Length)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return component.ExecuteAsync(command, response, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}
