using FluentAssertions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace CrossCutting.Common.Testing
{
    [ExcludeFromCodeCoverage]
    public static class TestHelpers
    {
        /// <summary>
        /// Asserts that the specified type performs argument null checks on all arguments.
        /// </summary>
        /// <param name="type">The type to assert null argument checks for.</param>
        /// <param name="parameterPredicate">Optional predicate to apply to each parameter info. When the predicate returns false, then the parameter will be skipped.</param>
        /// <param name="parameterReplaceDelegate">Optional function to apply to a parameter info. When the predicate is not defined, then we will create a mock or value type.</param>
        public static void ConstructorMustThrowArgumentNullException(Type type, Func<ParameterInfo, bool> parameterPredicate = null, Func<ParameterInfo, object> parameterReplaceDelegate = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters().ToArray();
                var mocks = GetMocks(parameters, parameterReplaceDelegate);

                for (int i = 0; i < parameters.Length; i++)
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
                            false.Should().BeTrue(string.Format("ArgumentNullException expected for parameter {0} of constructor, but no exception was thrown", parameters[i].Name));
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        ex.InnerException.Should().BeOfType<ArgumentNullException>();
                    }
                }
            }
        }

        private static object FillParameter(ParameterInfo[] parameters, int i)
            => parameters[i].ParameterType.IsValueType
                ? Activator.CreateInstance(parameters[i].ParameterType)
                : null;

        private static bool ShouldSkipParameter(Func<ParameterInfo, bool> parameterPredicate, ParameterInfo[] parameters, int i)
            => parameterPredicate != null
                && !parameterPredicate.Invoke(parameters[i]);

        public static void ConstructorShouldConstruct(Type type, Func<ParameterInfo, object> parameterReplaceDelegate = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters();
                var mocks = GetMocks(parameters, parameterReplaceDelegate);

                var actual = constructor.Invoke(mocks);
                actual.Should().BeOfType(type);
            }
        }

        private static object[] GetMocks(ParameterInfo[] parameters, Func<ParameterInfo, object> parameterReplaceDelegate)
            => parameters.Select
            (
                p =>
                {
                    if (parameterReplaceDelegate != null)
                    {
                        var returnValue = parameterReplaceDelegate.Invoke(p);
                        if (returnValue != null)
                        {
                            return returnValue;
                        }
                    }

                    if (p.ParameterType.IsValueType || p.ParameterType.IsArray || p.ParameterType == typeof(string))
                    {
                        return null; //skip value types, arrays and strings
                    }
                    var mockType = typeof(Mock<>).MakeGenericType(new[] { p.ParameterType });
                    var mock = (Mock)Activator.CreateInstance(mockType);
                    return mock.Object;
                }
            ).ToArray();

        private static void FixStringsAndArrays(ParameterInfo[] parameters, int i, object[] mocksCopy)
        {
            for (int j = 0; j < parameters.Length; j++)
            {
                if (j == i)
                {
                    continue;
                }
                if (parameters[j].ParameterType.IsArray)
                {
                    mocksCopy[j] = Activator.CreateInstance(parameters[j].ParameterType, new object[] { 1 });
                }
                else if (parameters[j].ParameterType == typeof(string))
                {
                    mocksCopy[j] = string.Empty;
                }
            }
        }

        /// <summary>
        /// Creates the object with the specified type using the specified dependency injection container.
        /// </summary>
        /// <param name="typeTocreate">Type to create</param>
        /// <param name="provider">Optional dependency injection container to use</param>
        /// <returns>Instanciated controller.</returns>
        public static object CreateObjectUsingDependecyInjection(Type typeTocreate, IServiceProvider provider = null)
        {
            if (typeTocreate == null)
            {
                throw new ArgumentNullException(nameof(typeTocreate));
            }

            // First try using dependency injection container
            var firstAttempt = provider?.GetService(typeTocreate);

            if (firstAttempt != null)
            {
                return firstAttempt;
            }

            // If this fails (controller types, for example), then construct the type and inject dependencies from the container
            var ctors = typeTocreate.GetConstructors();
            if (ctors?.Length == 0)
            {
                false.Should().BeTrue($"There is no constructor on type {typeTocreate.FullName}");
            }

            // Assumption: If there are more constructors, then use the first one
            return ctors?[0].Invoke
            (
                ctors[0].GetParameters()
                        .Select
                        (
                            parameterInfo => parameterInfo.ParameterType.IsValueType
                                // For value types, return a default value (like 0 for int or false for bool)
                                ? Activator.CreateInstance(parameterInfo.ParameterType)
                                // For reference types, resolve the service using dependency injection
                                : provider?.GetService(parameterInfo.ParameterType)
                        ).ToArray()
            );
        }
    }
}
