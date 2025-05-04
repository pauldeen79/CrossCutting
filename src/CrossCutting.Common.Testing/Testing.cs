using System.Reflection;
using System;

namespace CrossCutting.Common.Testing;

public static class Testing
{
    public static T CreateInstance<T>(Func<Type, object?> classFactory,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
    => (T)typeof(T).CreateInstance(classFactory, parameterReplaceDelegate, constructorPredicate)!;
}
