﻿namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class PrimitiveOrEnumDumper : IObjectDumperPart
{
    public int Order => 35;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        if (instance is null)
        {
            return false;
        }

        var type = instance.GetType();
        if (type.IsPrimitive || type.IsEnum)
        {
            builder.AddSingleValue(instance, instance.GetType());

            return true;
        }

        return false;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
