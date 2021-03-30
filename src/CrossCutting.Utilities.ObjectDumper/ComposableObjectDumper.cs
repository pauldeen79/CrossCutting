using CrossCutting.Utilities.ObjectDumper.Contracts;
using CrossCutting.Utilities.ObjectDumper.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CrossCutting.Utilities.ObjectDumper
{
    public class ComposableObjectDumper : IObjectDumperCallback
    {
        private readonly IObjectDumperPart[] _parts;

        public ComposableObjectDumper()
        {
            _parts = GetDefaultObjectDumpCustomizers(this).OrderBy(x => x.Order).ToArray();
        }

        public ComposableObjectDumper(params IObjectDumperPart[] customTypeHandlers)
            : this((IEnumerable<IObjectDumperPart>)customTypeHandlers)
        {
        }

        public ComposableObjectDumper(IEnumerable<IObjectDumperPart> customTypeHandlers)
        {
            _parts = GetDefaultObjectDumpCustomizers(this)
                .Concat(customTypeHandlers)
                .OrderBy(x => x.Order)
                .ToArray();
        }

        private static IEnumerable<IObjectDumperPart> GetDefaultObjectDumpCustomizers(IObjectDumperCallback callback)
            => typeof(ComposableObjectDumper)
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
                .Select(t => CreatePart(t, callback));

        private static IObjectDumperPart CreatePart(Type type, IObjectDumperCallback callback)
            => ((IObjectDumperPart)Activator.CreateInstance(type))
                .PerformActionOnType<IObjectDumperPart, IObjectDumperPartWithCallback>
                (a => a.Callback = callback);

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            try
            {
                foreach (var part in _parts)
                {
                    if (part is IObjectDumperPartWithCallback initializer && initializer.Callback == null)
                    {
                        initializer.Callback = this;
                    }
                    instance = part.Transform(instance, builder, indent, currentDepth);
                }

                if (!_parts.All(part => part.ShouldProcess(instance, builder, indent, currentDepth)))
                {
                    return true;
                }

                return Array.Find(_parts, part => part.Process(instance, instanceType, builder, indent, currentDepth)) != null;
            }
            catch (Exception ex)
            {
                builder.AddException(ex);

                return true;
            }
        }

        IEnumerable<PropertyDescriptor> IObjectDumperCallback.GetProperties(object instance)
            => _parts.Aggregate
                (
                    GetPropertyDescriptors(instance),
                    (seed, part) => part.ProcessProperties(seed)
                );

        IEnumerable<FieldInfo> IObjectDumperCallback.GetFields(object instance)
            => instance
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fi => !fi.Name.EndsWithAny(">i__Field", ">k__BackingField"));

        private IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object instance)
            => TypeDescriptor
                .GetProperties(instance)
                .Cast<PropertyDescriptor>()
                .Where(prop => _parts.All(part => part.ShouldProcessProperty(instance, prop)));
    }
}
