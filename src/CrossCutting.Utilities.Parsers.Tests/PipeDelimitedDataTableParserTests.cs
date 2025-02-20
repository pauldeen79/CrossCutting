namespace CrossCutting.Utilities.Parsers.Tests;

public class PipeDelimitedDataTableParserTests
{
    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input()
    {
        // Arrange
        const string input = "Value 1|Value 2|Value 3";

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input).ToArray();

        // Assert
        AssertParseResult(actual);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Two_Lines()
    {
        // Arrange
        const string input = @"Column 1|Column 2|Column 3
--------|--------|--------
Value 1|Value 2|Value 3";

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 2, 0, 0, 0).ToArray();

        // Assert
        AssertParseResult(actual);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Line_And_Outer_Columns()
    {
        // Arrange
        const string input = @"|Column 1|Column 2|Column 3|
|Value 1|Value 2|Value 3|";

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 1, 1, 1, 0).ToArray();

        // Assert
        AssertParseResult(actual);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Line_And_Outer_Columns_FormatValue()
    {
        // Arrange
        const string input = @"| Column 1 | Column 2 | Column 3 |
| Value 1  | Value 2  | Value 3  |";

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 1, 1, 1, 0, null, (_, value) => value.Trim()).ToArray();

        // Assert
        AssertParseResult(actual);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Line_And_Outer_Columns_FormatValue_And_ColumnNames()
    {
        // Arrange
        const string input = @"| Column 1 | Column 2 | Column 3 |
| Value 1  | Value 2  | Value 3  |";
        var columnNames = new[] { "Column 1", "Column 2", "Column 3" };

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 1, 1, 1, 0, columnNames, (_, value) => value.Trim()).ToArray();

        // Assert
        AssertParseResult(actual, columnNames);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Line_And_Outer_Columns_FormatValue_And_ColumnNames_In_Data()
    {
        // Arrange
        const string input = @"| Column 1 | Column 2 | Column 3 |
| Value 1  | Value 2  | Value 3  |";
        var columnNames = new[] { "Column 1", "Column 2", "Column 3" };

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 1, 1, 1, 1, null, (_, value) => value.Trim()).ToArray();

        // Assert
        AssertParseResult(actual, columnNames);
    }

    [Fact]
    public void Parse_Returns_Valid_Result_With_Simple_Input_Skip_First_Line_And_Outer_Columns_Advanced_FormatValue_And_ColumnNames_In_Data()
    {
        // Arrange
        const string input = @"| Column 1 | Column 2 | Column 3 |
| Value 1  | Value 2  |        3 |";

        static object formatFunction(string columnName, string value)
        {
            if (columnName.Length == 1)
            {
                // first line, columnName contains auto-generated (numeric, one-based) column index. we want to trim the value, which will be used as column name for the second row.
                return value.Trim();
            }
            // second line, columnName contains the parsed column name. we want to parse to int in case it's the third column.
            if (columnName == "Column 3")
            {
                // third column
                return int.Parse(value.Trim());
            }
            // first or second column
            return value.Trim();
        }

        // Act
        var actual = PipeDelimitedDataTableParser.Parse(input, 1, 1, 1, 1, null, formatFunction).ToArray();

        // Assert
        actual.ShouldNotBeNull().Length.ShouldBe(1, "One row should have been created");
        actual[0].Values.Count().ShouldBe(3, "Three columns should have been created");
            Enumerable
                .Range(0, 3)
                .ToList()
                .ForEach(index => actual[0].Values.ElementAt(index).Key.ShouldBe($"Column {index + 1}", "Our provided column names should have been used"));
            actual[0].Values.ElementAt(2).Value.ShouldBeOfType<int>("Third column should have been parsed into an integer by our format function");
    }

    private static void AssertParseResult(ParseResult<string, object>[] actual, string[]? columnNames = null)
    {
        if (columnNames is null)
        {
            columnNames = ["1", "2", "3"];
        }

        actual.ShouldNotBeNull().Length.ShouldBe(1);

        var firstRow = actual.FirstOrDefault();

        firstRow.ShouldNotBeNull();
        if (firstRow is not null)
        {
            firstRow.IsSuccessful.ShouldBeTrue();
            firstRow.ErrorMessages.ShouldBeEmpty();

            var contents = string.Join("|", firstRow.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));

            contents.ShouldBe($"{columnNames[0]};Value 1|{columnNames[1]};Value 2|{columnNames[2]};Value 3");
        }
    }
}
