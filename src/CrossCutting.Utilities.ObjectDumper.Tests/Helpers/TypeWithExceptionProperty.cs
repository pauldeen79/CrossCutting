namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

internal sealed class TypeWithExceptionProperty
{
    public string? Name { get; set; }
    public string Error => throw new InvalidOperationException();
}
