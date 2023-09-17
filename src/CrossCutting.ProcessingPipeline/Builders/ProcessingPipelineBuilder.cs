namespace CrossCutting.ProcessingPipeline.Builders;

public partial class ProcessingPipelineBuilder : IValidatableObject
{
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
    public List<Entities.IPipelineFeature> Features
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
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

    public ProcessingPipelineBuilder AddFeatures(IEnumerable<Entities.IPipelineFeature> features)
    {
        return AddFeatures(ArgumentGuard.IsNotNull(features, nameof(features)).ToArray());
    }

    public ProcessingPipelineBuilder AddFeatures(params Entities.IPipelineFeature[] features)
    {
        Features.AddRange(ArgumentGuard.IsNotNull(features, nameof(features)));
        return this;
    }

    public ProcessingPipelineBuilder AddFeature(Entities.IPipelineFeature feature)
    {
        Features.Add(ArgumentGuard.IsNotNull(feature, nameof(feature)));
        return this;
    }

    public ProcessingPipelineBuilder()
    {
        Features = new();
    }

    public ProcessingPipelineBuilder(Entities.ProcessingPipeline source)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));
        Features = source.Features.ToList();
    }
}
