using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace CrossCutting.Utilities.Parsers.Tests
{
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
            actual.IsSuccessful.Should().BeTrue();
            actual.ErrorMessages.Should().BeEmpty();
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be("0;world|1;!");
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
            actual.IsSuccessful.Should().BeFalse();
            actual.ErrorMessages.Should().HaveCount(1);
            actual.ErrorMessages.ElementAt(0).Should().Be("Warning: Format value 1 was not found in format placeholders");
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be("0;world|1;!");
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
            actual.IsSuccessful.Should().BeFalse();
            actual.ErrorMessages.Should().HaveCount(1);
            actual.ErrorMessages.ElementAt(0).Should().Be("Format placeholders count (2) is not equal to column values count (1), see #MISSING# in format values list (values)");
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be("0;world|1;#MISSING#");
        }

        [Fact]
        public void CanParseFormatStringWithFormatter()
        {
            // Arrange
            const string FormatString = "Hello {0}{1} on {2:dd-MM-yyyy}";
            var args = new object[] { "world", "!", new DateTime(2018, 1, 1) }.ToArray();

            // Act
            var actual = StringFormatParser.Parse(FormatString, args);

            // Assert
            actual.IsSuccessful.Should().BeTrue();
            actual.ErrorMessages.Should().BeEmpty();
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be($"0;world|1;!|2;{new DateTime(2018, 1, 1)}");
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
            actual.IsSuccessful.Should().BeTrue();
            actual.ErrorMessages.Should().BeEmpty();
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be("0;world|1;!");
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
            actual.IsSuccessful.Should().BeTrue();
            actual.ErrorMessages.Should().BeEmpty();
            var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            contents.Should().Be("0;\"world\"|1;\"!\"");
        }

        [Fact]
        public void CanUseParsedArgumentsStringForStringFormat()
        {
            // Arrange
            const string FormatString = "Hello, {0}{1}";
            //const string ArgumentsString = "\"John Doe\", \"!\"";
            //var parsedArguments = StringFormatParser.ParseWithArgumentsString(FormatString, ArgumentsString).Values.Select(kvp => kvp.Value.ToString().Replace("\"", "")).ToArray(); //note that we have to replace the double quotes...
            const string ArgumentsString = "John Doe, !";
            var parsedArguments = StringFormatParser.ParseWithArgumentsString(FormatString, ArgumentsString).Values.Select(kvp => kvp.Value.ToString()).ToArray(); //note that we have to replace the double quotes...

            // Act
            var actual = string.Format(FormatString, parsedArguments);

            // Assert
            actual.Should().Be("Hello, John Doe!");
        }
    }
}
