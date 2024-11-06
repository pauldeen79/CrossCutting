namespace CrossCutting.ProcessingPipeline;

public abstract class PipelineBuilderBase<T, TResult>
    where TResult : PipelineBuilderBase<T, TResult>
{
#pragma warning disable CA2227 // Collection properties should be read only
    [Required][ValidateObject]
    public IList<IBuilder<T>> Components
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get;
        set;
    }

    public TResult AddComponents(IEnumerable<IBuilder<T>> components)
    {
        ArgumentGuard.IsNotNull(components, nameof(components));

        return AddComponents(components.ToArray());
    }

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
        ArgumentGuard.IsNotNull(component, nameof(component));

        Components.Add(component);

        return (TResult)this;
    }

    public TResult ReplaceComponent<TOriginal>(IBuilder<T> newComponent)
    {
        ArgumentGuard.IsNotNull(newComponent, nameof(newComponent));

        return RemoveComponent<TOriginal>().AddComponent(newComponent);
    }

    public TResult RemoveComponent<TRemove>()
    {
        foreach (var component in Components.Where(x => x?.GetType() == typeof(TRemove)).ToArray())
        {
            Components.Remove(component);
        }

        return (TResult)this;
    }

    protected PipelineBuilderBase()
        => Components = new List<IBuilder<T>>();

    protected PipelineBuilderBase(IEnumerable<IBuilder<T>> components)
        => Components = components.ToList();
}
