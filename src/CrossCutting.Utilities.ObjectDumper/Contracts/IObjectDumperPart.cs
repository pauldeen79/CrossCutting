using System.Collections.Generic;
using System.ComponentModel;

namespace CrossCutting.Utilities.ObjectDumper.Contracts
{
    public interface IObjectDumperPart : IObjectDumper
    {
        bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth);
        bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor);
        int Order { get; }
        object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth);
        IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source);
    }
}
