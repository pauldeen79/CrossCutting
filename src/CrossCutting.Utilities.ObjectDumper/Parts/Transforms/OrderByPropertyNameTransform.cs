using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Transforms
{
    public class OrderByPropertyNameTransform : IObjectDumperPartWithCallback
    {
        public IObjectDumperCallback Callback { get; set; }

        private readonly string _typeName;
        private readonly Func<Type, bool> _typeFilter;

        public OrderByPropertyNameTransform(string typeName)
        {
            _typeName = typeName;
        }

        public OrderByPropertyNameTransform(Func<Type, bool> typeFilter)
        {
            _typeFilter = typeFilter;
        }

        public int Order => 29;

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth) => false;

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source)
        {
            var shouldTransform = false;

            if (_typeName != null && source.Any() && source.First().ComponentType.FullName == _typeName)
            {
                shouldTransform = true;
            }

            if (_typeFilter != null && source.Any() && _typeFilter(source.First().ComponentType))
            {
                shouldTransform = true;
            }

            if (!shouldTransform)
            {
                return source;
            }

            return source.OrderBy(p => p.Name);
        }

        public bool ShouldProcess(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object instance, PropertyDescriptor propertyDescriptor) => true;

        public object Transform(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
