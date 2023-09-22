namespace CrossCutting.ProcessingPipeline;

public abstract class PipelineBuilderBase<T, TResult>
    where TResult : PipelineBuilderBase<T, TResult>
{
#pragma warning disable CA2227 // Collection properties should be read only
    public IList<IBuilder<T>> Features
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public TResult AddFeatures(IEnumerable<IBuilder<T>> features)
        => AddFeatures(features.IsNotNull(nameof(features)).ToArray());

    public TResult AddFeatures(params IBuilder<T>[] features)
    {
        foreach (var feature in features.IsNotNull(nameof(features)))
        {
            Features.Add(feature);
        }

        return (TResult)this;
    }

    public TResult AddFeature(IBuilder<T> feature)
    {
        Features.Add(feature.IsNotNull(nameof(feature)));
        return (TResult)this;
    }

    public TResult ReplaceFeature<TOriginal>(IBuilder<T> newFeature)
        => RemoveFeature<TOriginal>().AddFeature(newFeature.IsNotNull(nameof(newFeature)));

    public TResult RemoveFeature<TRemove>()
    {
        foreach (var feature in Features.Where(x => x?.GetType() == typeof(TRemove)).ToArray())
        {
            Features.Remove(feature);
        }

        return (TResult)this;
    }

    protected IEnumerable<ValidationResult> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance.IsNotNull(nameof(instance)), new ValidationContext(instance, null, null), results, true);
        return results;
    }

    protected PipelineBuilderBase()
        => Features = new List<IBuilder<T>>();

    protected PipelineBuilderBase(IEnumerable<IBuilder<T>> features)
        => Features = features.ToList();
}
