using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class MyType
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
    }
}
