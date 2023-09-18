namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TModel> : IValidatableObject
{
#pragma warning disable CA2227 // Collection properties should be read only
    public IList<IPipelineFeature<TModel>> Features
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public Pipeline<TModel> Build() => new Pipeline<TModel>(Features);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new PipelineBase<TModel>(Features);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public PipelineBuilder<TModel> AddFeatures(IEnumerable<IPipelineFeature<TModel>> features) => AddFeatures(features.IsNotNull(nameof(features)).ToArray());

    public PipelineBuilder<TModel> AddFeatures(params IPipelineFeature<TModel>[] features)
    {
        foreach (var feature in features.IsNotNull(nameof(features)))
        {
            Features.Add(feature);
        }

        return this;
    }

    public PipelineBuilder<TModel> AddFeature(IPipelineFeature<TModel> feature)
    {
        Features.Add(feature.IsNotNull(nameof(feature)));
        return this;
    }

    public PipelineBuilder<TModel> ReplaceFeature<T>(IPipelineFeature<TModel> newFeature)
    {
        newFeature = newFeature.IsNotNull(nameof(newFeature));
        foreach (var feature in Features.Where(x => x.GetType() == typeof(T)).ToArray())
        {
            Features.Remove(feature);
        }

        AddFeature(newFeature);
        return this;
    }

    public PipelineBuilder() => Features = new List<IPipelineFeature<TModel>>();

    public PipelineBuilder(Pipeline<TModel> source)
    {
        source = source.IsNotNull(nameof(source));
        Features = source.Features.ToList();
    }
}

public class PipelineBuilder<TModel, TContext> : IValidatableObject
{
#pragma warning disable CA2227 // Collection properties should be read only
    public IList<IPipelineFeature<TModel, TContext>> Features
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public Pipeline<TModel, TContext> Build() => new Pipeline<TModel, TContext>(Features);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new PipelineBase<TModel, TContext>(Features);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public PipelineBuilder<TModel, TContext> AddFeatures(IEnumerable<IPipelineFeature<TModel, TContext>> features) => AddFeatures(features.IsNotNull(nameof(features)).ToArray());

    public PipelineBuilder<TModel, TContext> AddFeatures(params IPipelineFeature<TModel, TContext>[] features)
    {
        foreach (var feature in features.IsNotNull(nameof(features)))
        {
            Features.Add(feature);
        }
        
        return this;
    }

    public PipelineBuilder<TModel, TContext> AddFeature(IPipelineFeature<TModel, TContext> feature)
    {
        Features.Add(feature.IsNotNull(nameof(feature)));
        return this;
    }

    public PipelineBuilder<TModel, TContext> ReplaceFeature<T>(IPipelineFeature<TModel, TContext> newFeature)
    {
        newFeature = newFeature.IsNotNull(nameof(newFeature));
        foreach (var feature in Features.Where(x => x.GetType() == typeof(T)).ToArray())
        {
            Features.Remove(feature);
        }

        AddFeature(newFeature);
        return this;
    }

    public PipelineBuilder() => Features = new List<IPipelineFeature<TModel, TContext>>();

    public PipelineBuilder(Pipeline<TModel, TContext> source)
    {
        source = source.IsNotNull(nameof(source));
        Features = source.Features.ToList();
    }
}
