namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TModel> : PipelineBuilderBase<IPipelineFeature<TModel>, PipelineBuilder<TModel>>, IPipelineBuilder<TModel>
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel> source) : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features.Select(x => x.ToBuilder()))
    {
    }

    public IPipeline<TModel> Build()
        => new Pipeline<TModel>(Initialize, Features.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel>(Features?.Select(x => x.Build())));


    protected virtual void Initialize(TModel model, PipelineContext<TModel> pipelineContext)
    {
    }
}

public class PipelineBuilder<TModel, TContext> : PipelineBuilderBase<IPipelineFeature<TModel, TContext>, PipelineBuilder<TModel, TContext>>, IPipelineBuilder<TModel, TContext>
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel, TContext> source) : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features.Select(x => x.ToBuilder()))
    {
    }

    public IPipeline<TModel, TContext> Build()
        => new Pipeline<TModel, TContext>(Initialize, Features.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel, TContext>(Features?.Select(x => x.Build())));

    protected virtual void Initialize(TModel model, PipelineContext<TModel, TContext> pipelineContext)
    {
    }
}
