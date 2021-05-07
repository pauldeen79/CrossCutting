using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CrossCutting.Utilities.ObjectDumper.Contracts;
using CrossCutting.Utilities.ObjectDumper.Extensions;

namespace CrossCutting.Utilities.ObjectDumper
{
    public class ComposableObjectDumper : IObjectDumperCallback
    {
        private readonly IObjectDumperPart[] _parts;

        public ComposableObjectDumper(IEnumerable<IObjectDumperPart> typeHandlers)
        {
            _parts = typeHandlers
                .Select(x => x.PerformActionOnType<IObjectDumperPart, IObjectDumperPartWithCallback>(a => a.Callback = this))
                .OrderBy(x => x.Order)
                .ToArray();
        }

        public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
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
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                .Where(fi => !fi.Name.EndsWithAny(">i__Field", ">k__BackingField"));

        private IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object instance)
            => TypeDescriptor
                .GetProperties(instance)
                .Cast<PropertyDescriptor>()
                .Where(prop => _parts.All(part => part.ShouldProcessProperty(instance, prop)));
    }
}
