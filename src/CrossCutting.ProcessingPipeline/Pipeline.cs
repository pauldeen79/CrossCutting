namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TModel> : PipelineBase<TModel>, IPipeline<TModel>
{
    private readonly Action<TModel, PipelineContext<TModel>> _validationDelegate;

    public Pipeline(Action<TModel, PipelineContext<TModel>> validationDelegate, IEnumerable<IPipelineFeature<TModel>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public Result<TModel> Process(TModel model)
    {
        var pipelineContext = new PipelineContext<TModel>(ArgumentGuard.IsNotNull(model, nameof(model)));

        _validationDelegate(model, pipelineContext);

        foreach (var feature in Features)
        {
            var result = feature.Process(pipelineContext);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Success(model);
    }
}

public class Pipeline<TModel, TContext> : PipelineBase<TModel, TContext>, IPipeline<TModel, TContext>
{
    private readonly Action<TModel, PipelineContext<TModel, TContext>> _validationDelegate;

    public Pipeline(Action<TModel, PipelineContext<TModel, TContext>> validationDelegate, IEnumerable<IPipelineFeature<TModel, TContext>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public Result<TModel> Process(TModel model, TContext context)
    {
        var pipelineContext = new PipelineContext<TModel, TContext>(model.IsNotNull(nameof(model)), context.IsNotNull(nameof(context)));

        _validationDelegate(model, pipelineContext);

        foreach (var feature in Features)
        {
            var result = feature.Process(pipelineContext);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Success(model);
    }
}
