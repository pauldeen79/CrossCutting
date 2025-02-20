namespace CrossCutting.Utilities.Parsers.Tests;

public class StringFormatParserTests
{
    [Fact]
    public void CanParseFormatString()
    {
        // Arrange
        const string FormatString = "Hello {0}{1}";
        var args = new object[] { "world", "!" }.ToArray();

        // Act
        var actual = StringFormatParser.Parse(FormatString, args);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("0;world|1;!");
    }

    [Fact]
    public void CanParseFormatStringWithMissingFormatSpecifiers()
    {
        // Arrange
        const string FormatString = "Hello {0}{1";
        var args = new object[] { "world", "!" }.ToArray();

        // Act
        var actual = StringFormatParser.Parse(FormatString, args);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.ElementAt(0).ShouldBe("Warning: Format value 1 was not found in format placeholders");
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("0;world|1;!");
    }

    [Fact]
    public void CanParseFormatStringWithMissingArguments()
    {
        // Arrange
        const string FormatString = "Hello {0}{1}";
        var args = new object[] { "world" }.ToArray();

        // Act
        var actual = StringFormatParser.Parse(FormatString, args);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.ElementAt(0).ShouldBe("Format placeholders count (2) is not equal to column values count (1), see #MISSING# in format values list (values)");
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("0;world|1;#MISSING#");
    }

    [Fact]
    public void CanParseFormatStringWithFormatter()
    {
        // Arrange
        const string FormatString = "Hello {0}{1} on {2:dd-MM-yyyy}";
        var args = new object[] { "world", "!", new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) }.ToArray();

        // Act
        var actual = StringFormatParser.Parse(FormatString, args);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe($"0;world|1;!|2;{new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)}");
    }

    [Fact]
    public void CanParseFormatStringWithArgumentUsedMultipleTimes()
    {
        // Arrange
        const string FormatString = "Hello {0}{1}{1}{1}";
        var args = new object[] { "world", "!" }.ToArray();

        // Act
        var actual = StringFormatParser.Parse(FormatString, args);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("0;world|1;!");
    }

    [Fact]
    public void CanParseFormatStringWithArgumentsString()
    {
        // Arrange
        const string FormatString = "Hello {0}{1}";
        const string Args = "\"world\", \"!\"";

        // Act
        var actual = StringFormatParser.ParseWithArgumentsString(FormatString, Args);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("0;\"world\"|1;\"!\"");
    }

    [Fact]
    public void CanUseParsedArgumentsStringForStringFormat()
    {
        // Arrange
        const string FormatString = "Hello, {0}{1}";
        const string ArgumentsString = "John Doe, !";
        var parsedArguments = StringFormatParser.ParseWithArgumentsString(FormatString, ArgumentsString).Values.Select(kvp => kvp.Value.ToString()).ToArray(); //note that we have to replace the double quotes...

        // Act
        var actual = string.Format(FormatString, parsedArguments);

        // Assert
        actual.ShouldBe("Hello, John Doe!");
    }

    [Fact]
    public void Calling_Parse_With_Empty_FormatString_Returns_Error()
    {
        // Arrange
        var input = "";

        // Act
        var actual = StringFormatParser.Parse(input, "A");

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Format string is empty");
    }

    [Fact]
    public void Calling_ParseWithArgumentsString_With_Empty_FormatString_Returns_Error()
    {
        // Arrange
        var input = "";

        // Act
        var actual = StringFormatParser.ParseWithArgumentsString(input, "A");

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Format string is empty");
    }

    [Fact]
    public void Calling_ParseWithArgumentsString_With_Empty_ArgumentsString_Returns_Error()
    {
        // Arrange
        var input = "";

        // Act
        var actual = StringFormatParser.ParseWithArgumentsString("something", input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Arguments string is empty");
    }

    [Fact]
    public void Calling_Parse_With_Too_Many_OpenBrackets_Returns_Error()
    {
        // Arrange
        var input = "{{";

        // Act
        var actual = StringFormatParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Too many open braces found");
    }

    [Fact]
    public void Calling_Parse_With_Too_Many_CloseBrackets_Returns_Error()
    {
        // Arrange
        var input = "}}";

        // Act
        var actual = StringFormatParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Too many close braces found");
    }

    [Fact]
    public void Calling_Parse_With_A_String_Without_Placeholders_Returns_Error()
    {
        // Arrange
        var input = "test";

        // Act
        var actual = StringFormatParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("No format placeholders were found");
    }
}
