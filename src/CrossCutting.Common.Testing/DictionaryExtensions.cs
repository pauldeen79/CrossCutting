using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CrossCutting.Common.Testing;

[ExcludeFromCodeCoverage]
public static class DictionaryExtensions
{
    public static T GetOrCreate<T>(this IDictionary<Type, object?> instance, Func<Type, object?> classFactory)
    {
        if (instance.ContainsKey(typeof(T)))
        {
            var returnValue = instance[typeof(T)]!;
            if (returnValue is Type t)
            {
                var typeInstance =  (T?)classFactory(t)
                    ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");
                instance[typeof(T)] = typeInstance;
                return typeInstance;
            }

            if (returnValue is object[] types && types.All(x => x is Type))
            {
                var typeInstances = types
                    .OfType<Type>()
                    .Select(x => classFactory(x))
                    .Cast<T>()
                    .ToArray();
                instance[typeof(T)] = typeInstances;
                return typeInstances.FirstOrDefault()
                    ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");
            }

            return (T)returnValue;
        }

        var newInstance = Testing.CreateInstance<T>(classFactory, instance)
            ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");

        instance[typeof(T)] = newInstance;

        return newInstance;
    }

    public static IEnumerable<T> GetOrCreateMultiple<T>(this IDictionary<Type, object?> instance, Func<Type, object?> classFactory)
    {
        if (instance.ContainsKey(typeof(T)))
        {
            var returnValue = instance[typeof(T)]!;
            if (returnValue is Type t)
            {
                var typeInstance = (T?)classFactory(t)
                    ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");
                instance[typeof(T)] = typeInstance;
                return [typeInstance];
            }

            if (returnValue is object[] types && types.All(x => x is Type))
            {
                var typeInstances = types
                    .OfType<Type>()
                    .Select(x => classFactory(x))
                    .Cast<T>()
                    .ToArray();
                instance[typeof(T)] = typeInstances;
                return typeInstances;
            }

            return (IEnumerable<T>)returnValue;
        }

        var newInstance = Testing.CreateInstance<T>(classFactory, instance)
            ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");

        instance[typeof(T)] = newInstance;

        return [newInstance];
    }
}
