using System;

namespace CrossCutting.Utilities.ObjectDumper.Contracts
{
    public interface IObjectDumperResultBuilder
    {
        void BeginNesting(int indent, Type? instanceType);
        void BeginComplexType(int indent, Type? instanceType);
        void BeginEnumerable(int indent, Type? instanceType);
        void EndComplexType(bool first, int indent, Type? instanceType);
        void AddEnumerableItem(bool first, int indent, bool isComplexType);
        void AddException(Exception exception);
        void AddName(int indent, string name);
        void AddSingleValue(object? value, Type? instanceType);
        void EndEnumerable(int indent, Type? instanceType);
    }
}
