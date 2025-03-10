﻿namespace CrossCutting.Utilities.ObjectDumper;

public static class DynamicPropertyManager
{
    public static DynamicPropertyDescriptor<TTargetType, TPropertyType> CreateProperty<TTargetType, TPropertyType>
    (
        string displayName,
        Func<TTargetType, TPropertyType> getter,
        Action<TTargetType, TPropertyType> setter,
        Attribute[] attributes
    ) => new(displayName, getter, setter, attributes);

    public static DynamicPropertyDescriptor<TTargetType, TPropertyType> CreateProperty<TTargetType, TPropertyType>
    (
        string displayName,
        Func<TTargetType, TPropertyType> getHandler,
        Attribute[]? attributes
    ) => new(displayName, getHandler, (t, p) => { }, attributes);
}

public sealed class DynamicPropertyManager<TTarget> : IDisposable
    where TTarget : class
{
    private readonly DynamicTypeDescriptionProvider provider;
    private readonly TTarget? target;

    public DynamicPropertyManager()
    {
        Type type = typeof(TTarget);

        provider = new DynamicTypeDescriptionProvider(type);
        TypeDescriptor.AddProvider(provider, type);
    }

    public DynamicPropertyManager(TTarget target)
    {
        this.target = target;

        provider = new DynamicTypeDescriptionProvider(typeof(TTarget));
        TypeDescriptor.AddProvider(provider, target);
    }

    public IList<PropertyDescriptor> Properties => provider.Properties;

    public void Dispose()
    {
        if (ReferenceEquals(target, null))
        {
            TypeDescriptor.RemoveProvider(provider, typeof(TTarget));
        }
        else
        {
            TypeDescriptor.RemoveProvider(provider, target);
        }
    }
}
