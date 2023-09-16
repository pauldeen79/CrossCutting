namespace CrossCutting.ProcessingPipeline.Builders;

public partial class ProcessingPipelineBuilder : IValidatableObject
{
    public IEnumerable<Entities.PipelineFeature> Features
    {
        get;
        set;
    }

    public Entities.ProcessingPipeline Build()
    {
        return new Entities.ProcessingPipeline(Features);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new Entities.ProcessingPipelineBase(Features);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, validationContext ?? new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public ProcessingPipelineBuilder AddFeatures(IEnumerable<Entities.PipelineFeature> features)
    {
        features = ArgumentGuard.IsNotNull(features, nameof(features));
        return AddFeatures(features.ToArray());
    }

    public ProcessingPipelineBuilder AddFeatures(params Entities.PipelineFeature[] features)
    {
        features = ArgumentGuard.IsNotNull(features, nameof(features));
        Features = Features.Concat(features);
        return this;
    }

    public ProcessingPipelineBuilder()
    {
        Features = Enumerable.Empty<Entities.PipelineFeature>();
    }

    public ProcessingPipelineBuilder(Entities.ProcessingPipeline source)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));
        Features = source.Features;
    }
}
