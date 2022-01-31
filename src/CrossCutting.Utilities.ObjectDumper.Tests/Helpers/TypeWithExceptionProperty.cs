namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

[ExcludeFromCodeCoverage]
internal class TypeWithExceptionProperty
{
    public string? Name { get; set; }
#pragma warning disable CA1822 // Mark members as static
    public string Error { get { throw new InvalidOperationException(); } }
#pragma warning restore CA1822 // Mark members as static
}
