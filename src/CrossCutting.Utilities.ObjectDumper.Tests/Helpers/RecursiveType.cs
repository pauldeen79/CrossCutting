namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

internal sealed class RecursiveType
{
    public string? Name { get; set; }
    public RecursiveType? Child { get; set; }
}
