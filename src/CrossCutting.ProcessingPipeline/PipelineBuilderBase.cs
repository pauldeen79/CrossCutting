﻿namespace CrossCutting.ProcessingPipeline;

public abstract class PipelineBuilderBase<T, TResult>
    where TResult : PipelineBuilderBase<T, TResult>
{
#pragma warning disable CA2227 // Collection properties should be read only
    public IList<T> Features
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public TResult AddFeatures(IEnumerable<T> features)
        => AddFeatures(features.IsNotNull(nameof(features)).ToArray());

    public TResult AddFeatures(params T[] features)
    {
        foreach (var feature in features.IsNotNull(nameof(features)))
        {
            Features.Add(feature);
        }

        return (TResult)this;
    }

    public TResult AddFeature(T feature)
    {
        Features.Add(feature.IsNotNull(nameof(feature)));
        return (TResult)this;
    }

    public TResult ReplaceFeature<TOriginal>(T newFeature)
    {
        newFeature = newFeature.IsNotNull(nameof(newFeature));

        foreach (var feature in Features.Where(x => x?.GetType() == typeof(TOriginal)).ToArray())
        {
            Features.Remove(feature);
        }

        AddFeature(newFeature);
        return (TResult)this;
    }

    protected IEnumerable<ValidationResult> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    protected PipelineBuilderBase()
        => Features = new List<T>();

    protected PipelineBuilderBase(IEnumerable<T> features)
        => Features = features.ToList();
}