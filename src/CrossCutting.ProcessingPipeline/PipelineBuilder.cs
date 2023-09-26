namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TModel> : PipelineBuilderBase<IPipelineFeature<TModel>, PipelineBuilder<TModel>>, IValidatableObject
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel> source) : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features.Select(x => x.ToBuilder()))
    {
    }

    public Pipeline<TModel> Build()
        => new Pipeline<TModel>(Features.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel>(Features?.Select(x => x.Build())));
}

public class PipelineBuilder<TModel, TContext> : PipelineBuilderBase<IPipelineFeature<TModel, TContext>, PipelineBuilder<TModel, TContext>>, IValidatableObject
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel, TContext> source) : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features.Select(x => x.ToBuilder()))
    {
    }

    public Pipeline<TModel, TContext> Build()
        => new Pipeline<TModel, TContext>(Features.Select(x => x.Build()));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel, TContext>(Features?.Select(x => x.Build())));
}
