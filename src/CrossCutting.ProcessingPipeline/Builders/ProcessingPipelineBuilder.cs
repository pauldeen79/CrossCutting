namespace CrossCutting.ProcessingPipeline.Builders;

public partial class ProcessingPipelineBuilder : IValidatableObject
{
    public IEnumerable<PipelineFeatureBuilder> Features
    {
        get;
        set;
    }

    public Entities.ProcessingPipeline Build()
    {
        return new Entities.ProcessingPipeline(Features.Select(x => x.Build()));
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new Entities.ProcessingPipelineBase(Features.Select(x => x.Build()));
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public ProcessingPipelineBuilder AddFeatures(IEnumerable<PipelineFeatureBuilder> features)
    {
        return AddFeatures(features.ToArray());
    }

    public ProcessingPipelineBuilder AddFeatures(params PipelineFeatureBuilder[] features)
    {
        Features = Features.Concat(features);
        return this;
    }

    public ProcessingPipelineBuilder()
    {
        Features = Enumerable.Empty<PipelineFeatureBuilder>();
    }

    public ProcessingPipelineBuilder(Entities.ProcessingPipeline source)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));
        Features = source.Features.Select(x => x.ToBuilder());
    }
}
