namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TModel> : PipelineBase<TModel>, IPipeline<TModel>
{
    private readonly Action<TModel, PipelineContext<TModel>> _validationDelegate;

    public Pipeline(Action<TModel, PipelineContext<TModel>> validationDelegate, IEnumerable<IPipelineComponent<TModel>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public async Task<Result> Process(TModel model, CancellationToken token)
    {
        var pipelineContext = new PipelineContext<TModel>(ArgumentGuard.IsNotNull(model, nameof(model)));

        _validationDelegate(model, pipelineContext);

        var results = await Task.WhenAll(Components.Select(x => x.Process(pipelineContext, token))).ConfigureAwait(false);
        return Array.Find(results, x => !x.IsSuccessful())
            ?? Result.Success();
    }
}

public class Pipeline<TModel, TContext> : PipelineBase<TModel, TContext>, IPipeline<TModel, TContext>
{
    private readonly Action<TModel, PipelineContext<TModel, TContext>> _validationDelegate;

    public Pipeline(Action<TModel, PipelineContext<TModel, TContext>> validationDelegate, IEnumerable<IPipelineComponent<TModel, TContext>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public async Task<Result> Process(TModel model, TContext context, CancellationToken token)
    {
        var pipelineContext = new PipelineContext<TModel, TContext>(model.IsNotNull(nameof(model)), context.IsNotNull(nameof(context)));

        _validationDelegate(model, pipelineContext);

        var results = await Task.WhenAll(Components.Select(x => x.Process(pipelineContext, token))).ConfigureAwait(false);
        return Array.Find(results, x => !x.IsSuccessful())
            ?? Result.Success();
    }
}
