namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

[ExcludeFromCodeCoverage]
public class MyImmutableType
{
    public string Name { get; }
    public int Age { get; }

    public MyImmutableType(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
