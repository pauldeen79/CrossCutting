using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CrossCutting.Common.Testing;

[ExcludeFromCodeCoverage]
public static class Testing
{
    public static T CreateInstance<T>(
        Func<Type, object?> classFactory,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
            => (T)typeof(T).CreateInstance(classFactory, parameterReplaceDelegate, constructorPredicate)!;

    public static T CreateInstance<T>(
        Func<Type, object?> classFactory,
        IDictionary<Type, object?> mocks,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
            => (T)typeof(T).CreateInstance(classFactory, mocks, parameterReplaceDelegate, constructorPredicate)!;
}
