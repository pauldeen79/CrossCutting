namespace CrossCutting.ProcessingPipeline;

public class PipelineHandler<TCommand> : ICommandHandler<TCommand>
{
    private readonly List<IPipelineComponentInterceptor> _interceptors;
    private readonly IEnumerable<IPipelineComponent<TCommand>> _components;

    public PipelineHandler(IEnumerable<IPipelineComponentInterceptor> interceptors, IEnumerable<IPipelineComponent<TCommand>> components)
    {
        ArgumentGuard.IsNotNull(interceptors, nameof(interceptors));
        ArgumentGuard.IsNotNull(components, nameof(components));

        _interceptors = interceptors.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
        _components = components.OrderBy(x => (x as IOrderContainer)?.Order).ToArray();
    }

    public async Task<Result> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

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
    }

    private async Task<Result> DoExecuteAsync(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return component.ExecuteAsync(command, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}

public class PipelineHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
{
    private readonly List<IPipelineComponentInterceptor> _interceptors;
    private readonly IPipelineResponseGenerator _responseGenerator;
    private readonly IEnumerable<IPipelineComponent<TCommand, TResponse>> _components;

    public PipelineHandler(IEnumerable<IPipelineComponentInterceptor> interceptors, IPipelineResponseGenerator responseGenerator, IEnumerable<IPipelineComponent<TCommand, TResponse>> components)
    {
        ArgumentGuard.IsNotNull(interceptors, nameof(interceptors));
        ArgumentGuard.IsNotNull(responseGenerator, nameof(responseGenerator));
        ArgumentGuard.IsNotNull(components, nameof(components));

        _interceptors = interceptors.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
        _responseGenerator = responseGenerator;
        _components = components.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
    }

    public async Task<Result<TResponse>> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

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
                    Result.Success(response.Value!),
                    errors => Result.Error<TResponse>(errors, "An error occured while processing the pipeline. See the inner results for more details.")
                );
            }).ConfigureAwait(false);
    }

    private async Task<Result> DoExecuteAsync(IPipelineComponent<TCommand, TResponse> component, TCommand command, TResponse response, ICommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return component.ExecuteAsync(command, response, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}
