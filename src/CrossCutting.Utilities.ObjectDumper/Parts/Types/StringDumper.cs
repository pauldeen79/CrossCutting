using System;
using System.Collections.Generic;
using System.ComponentModel;
using CrossCutting.Utilities.ObjectDumper.Contracts;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Types
{
    public class StringDumper : IObjectDumperPart
    {
        public int Order => 20;

        public bool Process(object? instance, Type? instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (instance is string)
            {
                builder.AddSingleValue(instance.ToString(), instance.GetType());

                return true;
            }

            return false;
        }

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

        public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
