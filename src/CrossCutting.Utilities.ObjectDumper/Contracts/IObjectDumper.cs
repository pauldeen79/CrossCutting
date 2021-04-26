using System;

namespace CrossCutting.Utilities.ObjectDumper.Contracts
{
    public interface IObjectDumper
    {
        bool Process(object? instance, Type? instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth);
    }
}
