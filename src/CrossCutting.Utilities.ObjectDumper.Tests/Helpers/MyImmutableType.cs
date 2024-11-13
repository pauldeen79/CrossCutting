namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

public class MyImmutableType(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;
}
