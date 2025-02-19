namespace CrossCutting.Common.Tests.Extensions;

public partial class StringExtensionsTests
{
    public class SplitCsv
    {
        [Fact]
        public void Empty_String_Results_In_Empty_Array()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var actual = input.SplitCsv();

            // Assert
            actual.ShouldBeEmpty();
        }

        [Fact]
        public void Can_Split_String_With_TextQualifier_Using_Delimiter_At_The_End()
        {
            // Arrange
            var input = "a,\"b,c\",";

            // Act
            var actual = input.SplitCsv();

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "a", "b,c", string.Empty });
        }

        [Theory,
            InlineData(@"Simple,A,B,C", new[] { "A", "B", "C" }),
            InlineData(@"Comma in one value,""A,B"",B,C", new[] { @"A,B", "B", "C" }),
            InlineData(@"Quote in value,""A"""""",B,C", new[] { @"A""", "B", "C" }),
            InlineData(@"Single quote and comma in value,""A"""",B"",B,C", new[] { @"A"",B", "B", "C" })]
        public void SplitCsv_Gives_Correct_Result(string input, string[] expected)
        {
            // Act
            var result = input.SplitCsv();

            // Assert
            result.Skip(1).ToArray().ShouldBeEquivalentTo(expected);
        }
    }
}
