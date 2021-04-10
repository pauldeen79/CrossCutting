using CrossCutting.Common.Testing;
using CrossCutting.DataTableDumper.Default;
using FluentAssertions;
using Xunit;

namespace CrossCutting.DataTableDumper.Tests
{
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
            var lines = TestHelpers.GetLines(actual);
            lines.Should().HaveCount(3);
            lines.Should().HaveElementAt(0, "| Name                        | Age |");
            lines.Should().HaveElementAt(1, "| Person A                    | 42  |");
            lines.Should().HaveElementAt(2, "| Person B with a longer name | 8   |");
        }

        private class MyClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
