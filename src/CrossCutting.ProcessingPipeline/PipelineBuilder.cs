namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TModel> : PipelineBuilderBase<IPipelineComponent<TModel>, PipelineBuilder<TModel>>, IPipelineBuilder<TModel>
{
    public PipelineBuilder()
    {
    }

    public IPipeline<TModel> Build()
        => new Pipeline<TModel>(Initialize, Components.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel>(Components?.Select(x => x.Build())));


    protected virtual void Initialize(TModel model, PipelineContext<TModel> pipelineContext)
    {
    }
}

public class PipelineBuilder<TModel, TContext> : PipelineBuilderBase<IPipelineComponent<TModel, TContext>, PipelineBuilder<TModel, TContext>>, IPipelineBuilder<TModel, TContext>
{
    public PipelineBuilder()
    {
    }

    public IPipeline<TModel, TContext> Build()
        => new Pipeline<TModel, TContext>(Initialize, Components.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel, TContext>(Components?.Select(x => x.Build())));

    protected virtual void Initialize(TModel model, PipelineContext<TModel, TContext> pipelineContext)
    {
    }
}
