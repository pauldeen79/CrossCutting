using System;
using System.Collections.Generic;

namespace CrossCutting.Common.Testing;

public static class DictionaryExtensions
{
    public static T GetOrCreate<T>(this IDictionary<Type, object?> instance, Func<Type, object?> classFactory)
    {
        if (instance.ContainsKey(typeof(T)))
        {
            return (T)instance[typeof(T)]!;
        }

        var newInstance = Testing.CreateInstance<T>(classFactory, instance)
            ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");

        instance[typeof(T)] = newInstance;

        return newInstance;
    }
}
