using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters
{
    public class PropertyTypeNameExclusionFilter : IObjectDumperPart
    {
        private readonly string _skipPropertyTypeName;

        public PropertyTypeNameExclusionFilter(string skipPropertyTypeName) => _skipPropertyTypeName = skipPropertyTypeName ?? throw new ArgumentNullException(nameof(skipPropertyTypeName));

        public int Order => 99;

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth) => false;

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object instance, PropertyDescriptor propertyDescriptor) => propertyDescriptor.PropertyType.FullName != _skipPropertyTypeName;

        public object Transform(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
