using System;
using System.Collections.Generic;
using System.ComponentModel;
using CrossCutting.Utilities.ObjectDumper.Contracts;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters
{
    public class ReflectionTypeNameExclusionFilter : IObjectDumperPart
    {
        public int Order => 99;

        public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth) => false;

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => !propertyDescriptor.PropertyType.FullName.StartsWith("System.Reflection");

        public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
