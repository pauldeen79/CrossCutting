using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CrossCutting.Utilities.ObjectDumper.Contracts
{
    public interface IObjectDumperCallback : IObjectDumper
    {
        IEnumerable<PropertyDescriptor> GetProperties(object instance);
        IEnumerable<FieldInfo> GetFields(object instance);
    }
}
