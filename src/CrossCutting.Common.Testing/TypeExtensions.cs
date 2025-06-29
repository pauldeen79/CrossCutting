using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CrossCutting.Common.Testing;

[ExcludeFromCodeCoverage]
public static class TypeExtensions
{
    private const string Add = nameof(List<object>.Add);

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
        var constructors = GetConstructors(type);

        foreach (var constructor in constructors.Where(c => ShouldProcessConstructor(constructorPredicate, c)))
        {
            var parameters = constructor.GetParameters().ToArray();
            var argumentInstances = GetArguments(new Dictionary<Type, object?>(), parameters, parameterReplaceDelegate, classFactory);

            for (var i = 0; i < parameters.Length; i++)
            {
                if (ShouldSkipParameter(parameterPredicate, parameters, i))
                {
                    continue;
                }
                var argumentInstancesCopy = argumentInstances.ToArray();
                argumentInstancesCopy[i] = FillParameter(parameters, i);

                FixStringsAndArrays(parameters, i, argumentInstancesCopy);

                try
                {
                    constructor.Invoke(argumentInstancesCopy);
                    VerifyValueType(parameters, i);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is not ArgumentNullException argumentNullException
                        || argumentNullException.ParamName != parameters[i].Name)
                    {
                        throw;
                    }
                }
            }
        }
    }

    private static void VerifyValueType(ParameterInfo[] parameters, int i)
    {
        if (!parameters[i].ParameterType.IsValueType)
        {
            throw new InvalidOperationException($"ArgumentNullException expected for parameter {parameters[i].Name} of constructor, but no exception was thrown");
        }
    }

    private static ConstructorInfo[] GetConstructors(Type type)
    {
        var constructors = type.GetConstructors();
        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Type {type.FullName} should have public constructors");
        }

        return constructors;
    }

    public static object? CreateInstance(
        this Type type,
        Func<Type, object?> classFactory,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
        => CreateInstance(type, classFactory, new Dictionary<Type, object?>(), parameterReplaceDelegate, constructorPredicate);

    public static object? CreateInstance(
        this Type type,
        Func<Type, object?> classFactory,
        IDictionary<Type, object?> classFactories,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
    {
        if (classFactories.ContainsKey(type))
        {
            // Ensure only one instance per type is created, and we don't call the class factory for a second time.
            var returnValue = classFactories[type];
            if (returnValue is not Type t)
            {
                return returnValue;
            }

            // Class factory dictionary has defined a type mapping, let's use this
            type = t;
        }

        if (type == typeof(string))
        {
            // Special case for string: don't try construction with char array, just supply an empty string
            return string.Empty;
        }

        if (type.IsInterface)
        {
            var returnValue = classFactory.Invoke(type);
            classFactories[type] = returnValue;
            return returnValue;
        }

        var constructors = type
            .GetConstructors()
            .Where(c => ShouldProcessConstructor(constructorPredicate, c))
            .ToArray();

        if (constructors.Length == 0)
        {
            // If there are no public constructors, let the DI framework (or manual class factory, whatever) handle this.
            return classFactory.Invoke(type);
        }

        // For now, let's just pick the first constructor that matches.
        // You can use a constructor predicate to narrow down the available constructors, so you can filter it down to exactly one.
        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        var argumentInstances = GetArguments(classFactories, parameters, parameterReplaceDelegate, classFactory);
        var argumentInstancesCopy = argumentInstances.ToArray();
        for (var i = 0; i < parameters.Length; i++)
        {
            if ((parameters[i].ParameterType.IsValueType || parameters[i].ParameterType.IsEnum) && argumentInstancesCopy[i] is null)
            {
                argumentInstancesCopy[i] = Activator.CreateInstance(parameters[i].ParameterType);
            }
            else if (parameters[i].ParameterType != typeof(Type) && argumentInstancesCopy[i] is Type t)
            {
                argumentInstancesCopy[i] = CreateInstanceInternal(classFactories, t);
            }
        }

        FixStringsAndArrays(parameters, -1, argumentInstancesCopy);

        return constructor.Invoke(argumentInstancesCopy);
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

    private static object?[] GetArguments(IDictionary<Type, object?> classFactories, ParameterInfo[] parameters, Func<ParameterInfo, object?>? parameterReplaceDelegate, Func<Type, object?> classFactory)
        => parameters.Select
        (
            p =>
            {
                if (classFactories.ContainsKey(p.ParameterType))
                {
                    var returnValue = classFactories[p.ParameterType];
                    if (returnValue is not Type t)
                    {
                        return returnValue;
                    }

                    return classFactory(t);
                }

                var replacement = parameterReplaceDelegate?.Invoke(p);
                if (replacement is not null)
                {
                    return replacement;
                }

                if (p.ParameterType == typeof(string))
                {
                    return string.Empty; // use string.Empty for string arguments, in case they require a null check
                }
                else if (p.ParameterType == typeof(StringBuilder))
                {
                    var builder = new StringBuilder();
                    classFactories[typeof(StringBuilder)] = builder;
                    return builder;
                }
                else if (IsEnumerable(p.ParameterType))
                {
                    var containedType = GetContainedType(p.ParameterType);
                    var returnValue = CreateGenericList(containedType, classFactory, classFactories);
                    classFactories[containedType] = returnValue;
                    return returnValue;
                }
                else if (p.ParameterType.IsValueType || p.ParameterType.IsEnum)
                {
                    var returnValue = default(object?);
                    classFactories[p.ParameterType] = returnValue;
                    return returnValue; // skip value types and enums, these are not mocked
                }
                else
                {
                    var returnValue = classFactory.Invoke(p.ParameterType);
                    classFactories[p.ParameterType] = returnValue;
                    return returnValue;
                }
            }
        ).ToArray();

    private static bool IsEnumerable(Type t)
        => (t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t))
        || t.IsArray;

    private static Type GetContainedType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        return type.GetGenericArguments().FirstOrDefault()
            ?? throw new InvalidOperationException("Could not determine contained type");
    }

    private static object CreateGenericList(Type containedType, Func<Type, object?> classFactory, IDictionary<Type, object?> classFactories)
    {
        Type listType = typeof(List<>).MakeGenericType(containedType);
        var addMethod = listType.GetMethod(Add);
        var returnValue = Activator.CreateInstance(listType);

        if (classFactories.ContainsKey(containedType))
        {
            var classFactoryItem = classFactories[containedType];
            Process(classFactories, addMethod, returnValue, classFactoryItem);

            classFactories[containedType] = returnValue;
            return returnValue;
        }

        var itemToAdd = CreateInstance(containedType, classFactory);
        Process(classFactories, addMethod, returnValue, itemToAdd);

        return returnValue;
    }

    private static void Process(IDictionary<Type, object?> classFactories, MethodInfo addMethod, object returnValue, object? classFactoryItem)
    {
        if (classFactoryItem is null)
        {
            return;
        }

        if (IsEnumerable(classFactoryItem.GetType()))
        {
            foreach (var item in (IEnumerable)classFactoryItem)
            {
                if (item is Type t)
                {
                    addMethod.Invoke(returnValue, [CreateInstanceInternal(classFactories, t)]);
                }
                else
                {
                    addMethod.Invoke(returnValue, [item]);
                }
            }

        }
        else if (classFactoryItem is Type t)
        {
            addMethod.Invoke(returnValue, [CreateInstanceInternal(classFactories, t)]);
        }
        else
        {
            addMethod.Invoke(returnValue, [classFactoryItem]);
        }
    }

    private static object? CreateInstanceInternal(IDictionary<Type, object?> classFactories, Type t)
        // Note that for now, we only support object creation 10 levels deep
        => CreateInstance(t,
            t2 => CreateInstance(t2,
            t3 => CreateInstance(t3,
            t4 => CreateInstance(t4,
            t5 => CreateInstance(t5,
            t6 => CreateInstance(t6,
            t7 => CreateInstance(t7,
            t8 => CreateInstance(t8,
            t9 => CreateInstance(t9,
            t10 => CreateInstance(t10, _ => null, classFactories), classFactories), classFactories), classFactories), classFactories), classFactories), classFactories), classFactories), classFactories), classFactories);

    private static void FixStringsAndArrays(ParameterInfo[] parameters, int i, object?[] argumentInstances)
    {
        for (var j = 0; j < parameters.Length; j++)
        {
            if (j == i)
            {
                continue;
            }
            if (parameters[j].ParameterType.IsArray)
            {
                argumentInstances[j] = Activator.CreateInstance(parameters[j].ParameterType, 0);
            }
            else if (parameters[j].ParameterType.FullName?.StartsWith("System.Collections.Generic.IEnumerable", StringComparison.InvariantCulture) == true && argumentInstances[j] is null)
            {
                // note that for now, we only allow generic Enumerables to work.
                // this needs to be extended to generic collections and lists of more types.
                argumentInstances[j] = Activator.CreateInstance(typeof(List<>).MakeGenericType(parameters[j].ParameterType.GetGenericArguments()[0]));
            }
            else if (parameters[j].ParameterType == typeof(string))
            {
                argumentInstances[j] = string.Empty;
            }
        }
    }
}
