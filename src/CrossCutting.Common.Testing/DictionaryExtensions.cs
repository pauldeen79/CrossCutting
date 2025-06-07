using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Common.Testing;

[ExcludeFromCodeCoverage]
public static class DictionaryExtensions
{
    public static T GetOrCreate<T>(this IDictionary<Type, object?> instance, Func<Type, object?> classFactory)
    {
        if (instance.ContainsKey(typeof(T)))
        {
            var returnValue = instance[typeof(T)]!;
            if (returnValue is not Type t)
            {
                return (T)returnValue;
            }

            return (T?)classFactory(t)
                ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");
        }

        var newInstance = Testing.CreateInstance<T>(classFactory, instance)
            ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");

        instance[typeof(T)] = newInstance;

        return newInstance;
    }
}
