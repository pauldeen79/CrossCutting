namespace CrossCutting.ProcessingPipeline;

public abstract class PipelineBuilderBase<T, TResult>
    where TResult : PipelineBuilderBase<T, TResult>
{
#pragma warning disable CA2227 // Collection properties should be read only
    public IList<IBuilder<T>> Components
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public TResult AddComponente(IEnumerable<IBuilder<T>> components)
        => AddComponents(components.IsNotNull(nameof(components)).ToArray());

    public TResult AddComponents(params IBuilder<T>[] components)
    {
        foreach (var component in components.IsNotNull(nameof(components)))
        {
            Components.Add(component);
        }

        return (TResult)this;
    }

    public TResult AddComponent(IBuilder<T> component)
    {
        Components.Add(component.IsNotNull(nameof(component)));
        return (TResult)this;
    }

    public TResult ReplaceComponent<TOriginal>(IBuilder<T> newComponent)
        => RemoveComponent<TOriginal>().AddComponent(newComponent.IsNotNull(nameof(newComponent)));

    public TResult RemoveComponent<TRemove>()
    {
        foreach (var component in Components.Where(x => x?.GetType() == typeof(TRemove)).ToArray())
        {
            Components.Remove(component);
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
        => Components = new List<IBuilder<T>>();

    protected PipelineBuilderBase(IEnumerable<IBuilder<T>> components)
        => Components = components.ToList();
}
