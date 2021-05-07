using System;
using System.Collections.Generic;
using System.ComponentModel;
using CrossCutting.Utilities.ObjectDumper.Contracts;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    internal class ExceptionThrowingPart : IObjectDumperPart
    {
        public int Order => 1; //make sure this part takes over everything :)

        public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            if (instanceType.IsValueType)
            {
                return false;
            }
            throw new InvalidOperationException();
        }

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

        public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
