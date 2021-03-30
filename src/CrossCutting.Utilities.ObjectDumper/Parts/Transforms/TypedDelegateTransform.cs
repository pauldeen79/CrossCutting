using CrossCutting.Utilities.ObjectDumper.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CrossCutting.Utilities.ObjectDumper.Parts.Transforms
{
    public class TypedDelegateTransform<T> : IObjectDumperPart
        where T : class
    {
        public int Order => 99;

        private readonly Func<T, object> _transformDelegate;

        public TypedDelegateTransform(Func<T, object> transformDelegate) =>
            _transformDelegate = transformDelegate ?? throw new ArgumentNullException(nameof(transformDelegate));

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth) => false;

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object instance, PropertyDescriptor propertyDescriptor) => true;

        public object Transform(object instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) =>
            instance is T
                ? _transformDelegate((T)instance)
                : instance;
    }
}
