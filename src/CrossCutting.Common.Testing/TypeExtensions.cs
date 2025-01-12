using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;

namespace CrossCutting.Common.Testing;

[ExcludeFromCodeCoverage]
public static class TypeExtensions
{
    /// <summary>
    /// Asserts that the specified type performs argument null checks on all arguments in all (public) constructors, with a factory delegate to create reference types.
    /// </summary>
    /// <remarks>Note that this method throws an exeption when there is no public constructor.</remarks>
    /// <param name="type">The type to assert null argument checks for.</param>
    /// <param name="classFactory">Factory delegate to create instances from types (likely mocks)</param>
    /// <param name="parameterPredicate">Optional predicate to apply to each parameter info. When the predicate returns false, then the parameter will be skipped.</param>
    /// <param name="parameterReplaceDelegate">Optional function to apply to a parameter info. When the predicate is not defined, then we will create a mock or value type.</param>
    /// <param name="constructorPredicate">Optional constructor predicate. If not providerd, all public constructors will be used</param>
    public static void ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
        this Type type,
        Func<Type, object?> classFactory,
        Func<ParameterInfo, bool>? parameterPredicate = null,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
    {
        var constructors = type.GetConstructors();
        constructors.Should().Match<ConstructorInfo[]?>(x => x != null && x.Length > 0, $"Type {type.FullName} should have public constructors");

        foreach (var constructor in constructors.Where(c => ShouldProcessConstructor(constructorPredicate, c)))
        {
            var parameters = constructor.GetParameters().ToArray();
            var mocks = GetMocks(parameters, parameterReplaceDelegate, classFactory);

            for (var i = 0; i < parameters.Length; i++)
            {
                if (ShouldSkipParameter(parameterPredicate, parameters, i))
                {
                    continue;
                }
                var mocksCopy = mocks.ToArray();
                mocksCopy[i] = FillParameter(parameters, i);

                FixStringsAndArrays(parameters, i, mocksCopy);

                try
                {
                    constructor.Invoke(mocksCopy);
                    if (!parameters[i].ParameterType.IsValueType)
                    {
                        ((ArgumentNullException?)null).Should().NotBeNull($"ArgumentNullException expected for parameter {parameters[i].Name} of constructor, but no exception was thrown");
                    }
                }
                catch (TargetInvocationException ex)
                {
                    ex.InnerException
                        .Should().BeOfType<ArgumentNullException>()
                        .And.Match<ArgumentNullException>(x => x.ParamName == parameters[i].Name);
                }
            }
        }
    }

    public static object? CreateInstance(
        this Type type,
        Func<Type, object?> classFactory,
        Func<ParameterInfo, object?>? parameterReplaceDelegate,
        Func<ConstructorInfo, bool>? constructorPredicate)
    {
        if (type.IsInterface)
        {
            return classFactory.Invoke(type);
        }

        var constructors = type.GetConstructors().Where(c => ShouldProcessConstructor(constructorPredicate, c)).ToArray();
        if (constructors.Length == 0)
        {
            // If there are no public constructors, let the DI framework (or manual class factory, whatever) handle this.
            return classFactory.Invoke(type);
        }

        // For now, let's just pick the first constructor that matches.
        // You can use a constructor predicate to narrow down the available constructors, so you can filter it down to exactly one.
        var constructor = constructors[0];
        var parameters = constructor.GetParameters().ToArray();
        var mocks = GetMocks(parameters, parameterReplaceDelegate, classFactory);
        var mocksCopy = mocks.ToArray();
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType.IsValueType)
            {
                mocksCopy[i] = Activator.CreateInstance(parameters[i].ParameterType);
            }
        }

        FixStringsAndArrays(parameters, -1, mocksCopy);

        return constructor.Invoke(mocksCopy);
    }

    private static bool ShouldProcessConstructor(Func<ConstructorInfo, bool>? constructorPredicate, ConstructorInfo c)
        => constructorPredicate is null
        || constructorPredicate(c);

    private static object? FillParameter(ParameterInfo[] parameters, int i)
        => parameters[i].ParameterType.IsValueType
            ? Activator.CreateInstance(parameters[i].ParameterType)
            : null;

    private static bool ShouldSkipParameter(Func<ParameterInfo, bool>? parameterPredicate, ParameterInfo[] parameters, int i)
        => parameterPredicate is not null
        && !parameterPredicate.Invoke(parameters[i]);

    private static object?[] GetMocks(ParameterInfo[] parameters, Func<ParameterInfo, object?>? parameterReplaceDelegate, Func<Type, object?> classFactory)
        => parameters.Select
        (
            p =>
            {
                if (parameterReplaceDelegate is not null)
                {
                    var returnValue = parameterReplaceDelegate.Invoke(p);
                    if (returnValue is not null)
                    {
                        return returnValue;
                    }
                }

                if (p.ParameterType == typeof(string))
                {
                    return string.Empty; // use string.Empty for string arguments, in case they require a null check
                }
                else if (p.ParameterType == typeof(StringBuilder))
                {
                    return new StringBuilder();
                }
                else if (p.ParameterType.IsValueType || p.ParameterType.IsArray)
                {
                    return null; //skip value types and arrays
                }
                else
                {
                    return classFactory.Invoke(p.ParameterType);
                }
            }
        ).ToArray();

    private static void FixStringsAndArrays(ParameterInfo[] parameters, int i, object?[] mocksCopy)
    {
        for (var j = 0; j < parameters.Length; j++)
        {
            if (j == i)
            {
                continue;
            }
            if (parameters[j].ParameterType.IsArray)
            {
                mocksCopy[j] = Activator.CreateInstance(parameters[j].ParameterType, 0);
            }
            else if (parameters[j].ParameterType.FullName?.StartsWith("System.Collections.Generic.IEnumerable", StringComparison.InvariantCulture) == true)
            {
                // note that for now, we only allow generic Enumerables to work.
                // this needs to be extended to generic collections and lists of more types.
                mocksCopy[j] = Activator.CreateInstance(typeof(List<>).MakeGenericType(parameters[j].ParameterType.GetGenericArguments()[0]));
            }
            else if (parameters[j].ParameterType == typeof(string))
            {
                mocksCopy[j] = string.Empty;
            }
        }
    }
}
