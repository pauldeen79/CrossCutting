using System;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    internal class TypeWithTypeProperty
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }
    }
}
