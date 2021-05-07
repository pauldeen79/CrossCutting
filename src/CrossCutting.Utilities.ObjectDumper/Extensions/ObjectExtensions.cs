using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections.Generic;

namespace CrossCutting.Utilities.ObjectDumper.Extensions
{
    public static class ObjectExtensions
    {
        public static string Dump<T>(this T instance, params IObjectDumperPart[] customTypeHandlers) =>
            Dump(instance, new DefaultObjectDumperResultBuilder(), customTypeHandlers);

        public static string Dump<T>(this T instance, IEnumerable<IObjectDumperPart> customTypeHandlers) =>
            Dump(instance, new DefaultObjectDumperResultBuilder(), customTypeHandlers);

        public static string Dump<T>(this T instance, IObjectDumperResultBuilder builder, params IObjectDumperPart[] customTypeHandlers) =>
            Dump(instance, builder, (IEnumerable<IObjectDumperPart>)customTypeHandlers);

        public static string Dump<T>(this T instance, IObjectDumperResultBuilder builder, IEnumerable<IObjectDumperPart> customTypeHandlers)
        {
            ComposableObjectDumperFactory.Create(customTypeHandlers).Process(instance, typeof(T), builder, 4, 1);

            return builder.ToString();
        }

        internal static T PerformActionOnType<T, TFilter>(this T instance, Action<TFilter> actionDelegate)
            where T : class
            where TFilter : class
        {
            if (instance is TFilter filterType)
            {
                actionDelegate(filterType);
            }

            return instance;
        }
    }
}
