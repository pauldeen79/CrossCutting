using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common.Extensions;
using CrossCutting.DataTableDumper.Default;
using CrossCutting.DataTableDumper.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.DataTableDumper.Tests
{
    [ExcludeFromCodeCoverage]
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
            var sut = new DataTableDumper<MyClass>(new ColumnNameProvider<MyClass>(), new ColumnDataProvider<MyClass>());

            // Act
            var actual = sut.Dump(input);

            // Assert
            var lines = actual.GetLines();
            lines.Should().HaveCount(3);
            lines.Should().HaveElementAt(0, "| Name                        | Age |");
            lines.Should().HaveElementAt(1, "| Person A                    | 42  |");
            lines.Should().HaveElementAt(2, "| Person B with a longer name | 8   |");
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
            var sut = new DataTableDumper<MyClass>(new ColumnNameProvider<MyClass>(), new ColumnDataProvider<MyClass>());

            // Act
            var actual = sut.Dump(input);

            // Assert
            var lines = actual.GetLines();
            lines.Should().HaveCount(3);
            lines.Should().HaveElementAt(0, "| Name                        | Age |");
            lines.Should().HaveElementAt(1, "| Person_A                    | 42  |");
            lines.Should().HaveElementAt(2, "| Person_B with a longer name | 8   |");
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
            lines.Should().HaveCount(7);
            lines.Should().HaveElementAt(0, "| Kolom A | Kolom B | Kolom C |");
            lines.Should().HaveElementAt(1, "| A       | 1       | z       |");
            lines.Should().HaveElementAt(2, "| B       | 2       | y       |");
            lines.Should().HaveElementAt(3, "| C       | 3       | x       |");
            lines.Should().HaveElementAt(4, "| D       | 4       | w       |");
            lines.Should().HaveElementAt(5, "| E       | 5       | v       |");
            lines.Should().HaveElementAt(6, "| F       | 6       | u       |");
        }

        [Fact]
        public void Can_Parse_Dumped_DataTable()
        {
            // Arrange
            var input = new[]
            {
                new MyClass { Name = "Person|A", Age = 42 },
            };
            var sut = new DataTableDumper<MyClass>(new ColumnNameProvider<MyClass>(), new ColumnDataProvider<MyClass>());
            var dumpedString = sut.Dump(input);

            // Act
            var lines = dumpedString.GetLines().Skip(1).Select(x => x.UnescapePipes()).ToArray();

            // Assert
            lines.Should().HaveCount(1);
            lines.Should().HaveElementAt(0, "| Person|A | 42  |");

        }

        [ExcludeFromCodeCoverage]
        public class MyClass
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }
    }
}
