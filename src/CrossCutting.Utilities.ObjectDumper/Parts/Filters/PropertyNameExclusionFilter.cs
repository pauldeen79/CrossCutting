﻿namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters;

public class PropertyNameExclusionFilter : IObjectDumperPart
{
    private readonly string? _skipPropertyName;
    private readonly string? _typeName;
    private readonly Func<Type, bool>? _typeFilter;

    public PropertyNameExclusionFilter(string skipPropertyName) => _skipPropertyName = skipPropertyName;

    public PropertyNameExclusionFilter(string skipPropertyName, string typeName)
    {
        _skipPropertyName = skipPropertyName;
        _typeName = typeName;
    }

    public PropertyNameExclusionFilter(string skipPropertyName, Func<Type, bool> typeFilter)
    {
        _skipPropertyName = skipPropertyName;
        _typeFilter = typeFilter;
    }

    public int Order => 99;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => propertyDescriptor.Name != _skipPropertyName
                      && (_typeName is null || propertyDescriptor.ComponentType.FullName == _typeName)
                      && (_typeFilter is null || _typeFilter(propertyDescriptor.ComponentType));

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
