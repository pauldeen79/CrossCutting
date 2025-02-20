namespace CrossCutting.DataTableDumper.Tests.TabDelimited;

public class ParserTests
{
    [Fact]
    public void Can_Parse_Excel_Table()
    {
        // Arrange
        var input = @"Kolom A	Kolom B	Kolom C
A	1	z
B	2	y
C	3	x
D	4	w
E	5	v
F	6	u"; //copied directly from Excel 8-)

        var result = DataTableDumper.TabDelimited.Parser.Parse(input);
        var sut = result.DataTableDumper;
        var list = result.List;

        // Act
        var actual = sut.Dump(list);

        // Assert
        var lines = actual.GetLines();
        lines.ShouldBeEquivalentTo(new[] {
            "| Kolom A | Kolom B | Kolom C |",
            "| A       | 1       | z       |",
            "| B       | 2       | y       |",
            "| C       | 3       | x       |",
            "| D       | 4       | w       |",
            "| E       | 5       | v       |",
            "| F       | 6       | u       |" }
        );
    }
}
