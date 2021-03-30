using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Types
{
    public class ComplexTypeDumper : IObjectDumperPartWithCallback
    {
        public IObjectDumperCallback Callback { get; set; }

        public int Order => 99;

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            if (instance == null)
            {
                return false;
            }

            builder.BeginNesting(indent, instanceType);
            builder.BeginComplexType(indent, instanceType);

            var first = IterateChildren
            (
                true,
                builder,
                indent,
                currentDepth,
                () => Callback.GetProperties(instance),
                property => ((PropertyDescriptor)property).Name,
                property => ((PropertyDescriptor)property).GetValue(instance),
                property => ((PropertyDescriptor)property).PropertyType
            );

            first = IterateChildren
            (
                first,
                builder,
                indent,
                currentDepth,
                () => Callback.GetFields(instance),
                property => ((FieldInfo)property).Name,
                property => ((FieldInfo)property).GetValue(instance),
                property => ((FieldInfo)property).FieldType
            );

            builder.EndComplexType(first, indent, instanceType);

            return true;
        }

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        private bool IterateChildren
        (
            bool first,
            IObjectDumperResultBuilder builder,
            int indent,
            int currentDepth,
            Func<IEnumerable> enumerableDelegate,
            Func<object, string> getNameDelegate,
            Func<object, object> getValueDelegate,
            Func<object, Type> getTypeDelegate
        )
        {
            foreach (var item in enumerableDelegate())
            {
                builder.AddEnumerableItem(first, indent, true);
                if (first)
                {
                    first = false;
                }

                builder.AddName(indent, getNameDelegate(item));

                try
                {
                    Callback.Process(getValueDelegate(item), getTypeDelegate(item), builder, indent + 4, currentDepth + 1);
                }
                catch (Exception ex)
                {
                    builder.AddException(ex);
                }
            }

            return first;
        }

        public bool ShouldProcess(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object instance, PropertyDescriptor propertyDescriptor) => true;

        public object Transform(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
