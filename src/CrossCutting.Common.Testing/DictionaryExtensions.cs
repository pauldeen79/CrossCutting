using System;
using System.Collections.Generic;

namespace CrossCutting.Common.Testing;

//[ExcludeFromCodeCoverage]
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

            if (returnValue is Type[] types)
            {
                var typeInstances = (T?)classFactory(types[0])
                    ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");
                instance[typeof(T)] = typeInstances;
                return typeInstances;
            }

            return (T)returnValue;
        }

        var newInstance = Testing.CreateInstance<T>(classFactory, instance)
            ?? throw new InvalidOperationException($"Class factory did not create an instance of type {typeof(T).FullName}");

        instance[typeof(T)] = newInstance;

        return newInstance;
    }
}
