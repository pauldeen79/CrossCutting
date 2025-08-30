namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

internal sealed class TypeWithExceptionProperty
{
    public string? Name { get; set; }
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public string Error => throw new InvalidOperationException();
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
}
