namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

internal class TypeWithExceptionProperty
{
    public string? Name { get; set; }
    public string Error => throw new InvalidOperationException();
}
