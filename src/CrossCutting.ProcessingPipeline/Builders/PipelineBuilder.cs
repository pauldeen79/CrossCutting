namespace CrossCutting.ProcessingPipeline.Builders;

public partial class PipelineBuilder : IValidatableObject
{
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
    public List<IPipelineFeature> Features
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
    {
        get;
        set;
    }

    public Pipeline Build()
    {
        return new Pipeline(Features);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new PipelineBase(Features);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public PipelineBuilder AddFeatures(IEnumerable<IPipelineFeature> features)
    {
        return AddFeatures(ArgumentGuard.IsNotNull(features, nameof(features)).ToArray());
    }

    public PipelineBuilder AddFeatures(params IPipelineFeature[] features)
    {
        Features.AddRange(ArgumentGuard.IsNotNull(features, nameof(features)));
        return this;
    }

    public PipelineBuilder AddFeature(IPipelineFeature feature)
    {
        Features.Add(ArgumentGuard.IsNotNull(feature, nameof(feature)));
        return this;
    }

    public PipelineBuilder ReplaceFeature<T>(IPipelineFeature newFeature)
    {
        newFeature = ArgumentGuard.IsNotNull(newFeature, nameof(newFeature));
        Features.RemoveAll(f => f.GetType() == typeof(T));
        Features.Add(newFeature);
        return this;
    }

    public PipelineBuilder()
    {
        Features = new();
    }

    public PipelineBuilder(Pipeline source)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));
        Features = source.Features.ToList();
    }
}
