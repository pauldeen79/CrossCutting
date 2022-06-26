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
        lines.Should().BeEquivalentTo(new[]
        {
            "| Name                        | Age |",
            "| Person A                    | 42  |",
            "| Person B with a longer name | 8   |"
        });
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
        lines.Should().BeEquivalentTo(new[]
        {
            "| Name                        | Age |",
            "| Person_A                    | 42  |",
            "| Person_B with a longer name | 8   |"
        });
    }

    [Fact]
    public void Can_Dump_ExcelTable()
    {
        // Arrange
        var input = @"Kolom A	Kolom B	Kolom C
A	1	z
B	2	y
C	3	x
D	4	w
E	5	v
F	6	u"; //copied directly from Excel 8-)

        var result = TabDelimited.Parser.Parse(input);
        var sut = result.DataTableDumper;
        var list = result.List;

        // Act
        var actual = sut.Dump(list);

        // Assert
        var lines = actual.GetLines();
        lines.Should().BeEquivalentTo(new[]
        {
            "| Kolom A | Kolom B | Kolom C |",
            "| A       | 1       | z       |",
            "| B       | 2       | y       |",
            "| C       | 3       | x       |",
            "| D       | 4       | w       |",
            "| E       | 5       | v       |",
            "| F       | 6       | u       |"
        });
    }

    [Fact]
    public void Can_Parse_Dumped_DataTable()
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
        lines.Should().BeEquivalentTo(new[]
        {
            "| Person|A | 42  |"
        });
    }

    public class MyClass
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
