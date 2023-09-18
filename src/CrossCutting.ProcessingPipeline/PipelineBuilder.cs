namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TModel> : PipelineBuilderBase<IPipelineFeature<TModel>, PipelineBuilder<TModel>>, IValidatableObject
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel> source)
        : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features)
    {
    }

    public Pipeline<TModel> Build()
        => new Pipeline<TModel>(Features);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel>(Features));
}

public class PipelineBuilder<TModel, TContext> : PipelineBuilderBase<IPipelineFeature<TModel, TContext>, PipelineBuilder<TModel, TContext>>, IValidatableObject
{
    public PipelineBuilder()
    {
    }

    public PipelineBuilder(Pipeline<TModel, TContext> source)
        : base(ArgumentGuard.IsNotNull(source, nameof(source)).Features)
    {
    }

    public Pipeline<TModel, TContext> Build()
        => new Pipeline<TModel, TContext>(Features);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(new PipelineBase<TModel, TContext>(Features));
}
