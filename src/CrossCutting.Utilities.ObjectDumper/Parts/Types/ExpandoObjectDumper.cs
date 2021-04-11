using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Types
{
    public class ExpandoObjectDumper : IObjectDumperPartWithCallback
    {
        public IObjectDumperCallback Callback { get; set; }

        public int Order => 30;

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!(instance is IEnumerable<KeyValuePair<string, object>> enumerable))
            {
                return false;
            }

            builder.BeginNesting(indent, instanceType);
            builder.BeginComplexType(indent, instanceType);

            bool first = true;
            foreach (var keyValuePair in enumerable)
            {
                builder.AddEnumerableItem(first, indent, true);
                if (first)
                {
                    first = false;
                }

                builder.AddName(indent, keyValuePair.Key);

                try
                {
                    Callback.Process(keyValuePair.Value, keyValuePair.Value?.GetType(), builder, indent + 4, currentDepth + 1);
                }
                catch (Exception ex)
                {
                    builder.AddException(ex);
                }
            }

            builder.EndComplexType(first, indent, typeof(IEnumerable<KeyValuePair<string, object>>));

            return true;
        }

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object instance, PropertyDescriptor propertyDescriptor) => true;

        public object Transform(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
