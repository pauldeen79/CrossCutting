using System;
using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class TypeWithTypeProperty
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }
    }
}
