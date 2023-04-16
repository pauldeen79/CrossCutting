namespace CrossCutting.DataTableDumper.Tests;

public class DataTableDumperTests
{
    [Fact]
    public void Can_Dump_DataTable()
    {
        // Arrange
        var input = new[]
        {
            new MyClass { Name = "Person A", Age = 42 },
            new MyClass { Name = "Person B with a longer name", Age = 8 }
        };
        var sut = new DataTableDumper<MyClass>(new ColumnNameProvider(), new ColumnDataProvider<MyClass>());

        // Act
        var actual = sut.Dump(input);

        // Assert
        var lines = actual.GetLines();
        lines.Should().BeEquivalentTo(
            "| Name                        | Age |",
            "| Person A                    | 42  |",
            "| Person B with a longer name | 8   |"
        );
    }

    [Fact]
    public void Can_Dump_DataTable_With_Pipes()
    {
        // Arrange
        var input = new[]
        {
            new MyClass { Name = "Person|A", Age = 42 },
            new MyClass { Name = "Person|B with a longer name", Age = 8 }
        };
        var sut = new DataTableDumper<MyClass>(new ColumnNameProvider(), new ColumnDataProvider<MyClass>());

        // Act
        var actual = sut.Dump(input);

        // Assert
        var lines = actual.GetLines();
        lines.Should().BeEquivalentTo(
            "| Name                        | Age |",
            "| Person_A                    | 42  |",
            "| Person_B with a longer name | 8   |"
        );
    }

    [Fact]
    public void Can_Parse_Dumped_DataTable_Into_StringArray()
    {
        // Arrange
        var input = new[]
        {
            new MyClass { Name = "Person|A", Age = 42 },
        };
        var sut = new DataTableDumper<MyClass>(new ColumnNameProvider(), new ColumnDataProvider<MyClass>());
        var dumpedString = sut.Dump(input);

        // Act
        var lines = dumpedString.GetLines().Skip(1).Select(x => x.UnescapePipes()).ToArray();

        // Assert
        lines.Should().BeEquivalentTo("| Person|A | 42  |");
    }

    [Fact]
    public void Can_Parse_Dumped_DataTable_Into_Typed_Array()
    {
        // Arrange
        var input = new[]
        {
            new MyClass { Name = "Person|A", Age = 42 },
        };
        var sut = new DataTableDumper<MyClass>(new ColumnNameProvider(), new ColumnDataProvider<MyClass>());
        var dumpedString = sut.Dump(input);

        // Act
        var lines = dumpedString.GetLines()
            .Skip(1)
            .Select(x => x.Split('|'))
            .Select(x => new MyClass { Name = x[1].Trim().UnescapePipes(), Age = int.Parse(x[2].Trim()) })
            .ToArray();

        // Assert
        lines.Should().BeEquivalentTo(input);
    }

    public record MyClass
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
