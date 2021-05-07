using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Utilities.ObjectDumper.Contracts;

namespace CrossCutting.Utilities.ObjectDumper
{
    public static class ComposableObjectDumperFactory
    {
        public static IObjectDumper Create(IEnumerable<IObjectDumperPart>? customTypeHandlers = null)
            => new ComposableObjectDumper
            (
                typeof(ComposableObjectDumper)
                    .Assembly
                    .GetExportedTypes()
                    .Where
                    (t =>
                        !t.IsAbstract
                        && !t.IsInterface
                        && !t.ContainsGenericParameters
                        && typeof(IObjectDumperPart).IsAssignableFrom(t)
                        && t.GetConstructor(Type.EmptyTypes) != null
                    )
                    .Select(t => CreatePart(t))
                    .Concat(customTypeHandlers ?? Enumerable.Empty<IObjectDumperPart>())
            );

        private static IObjectDumperPart CreatePart(Type type)
            => (IObjectDumperPart)Activator.CreateInstance(type);
    }
}
