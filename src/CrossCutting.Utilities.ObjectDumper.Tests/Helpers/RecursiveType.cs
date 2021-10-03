using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class RecursiveType
    {
        public string? Name { get; set; }
        public RecursiveType? Child { get; set; }
    }
}
