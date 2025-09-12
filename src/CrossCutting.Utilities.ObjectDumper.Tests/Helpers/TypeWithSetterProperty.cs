namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

internal sealed class TypeWithSetterProperty
{
    public string? Name { get; set; }
#pragma warning disable S2376 // Write-only properties should not be used
#pragma warning disable S3237 // "value" parameters should be used
#pragma warning disable S108 // Nested blocks of code should not be left empty
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
#pragma warning disable S1186 // Methods should not be empty
    public string Error { set { } }
#pragma warning restore S1186 // Methods should not be empty
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S108 // Nested blocks of code should not be left empty
#pragma warning restore S3237 // "value" parameters should be used
#pragma warning restore S2376 // Write-only properties should not be used
}
