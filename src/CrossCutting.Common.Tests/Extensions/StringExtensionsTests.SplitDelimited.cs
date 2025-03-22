namespace CrossCutting.Common.Tests.Extensions;

public partial class StringExtensionsTests
{
    public class SplitDelimited
    {
        [Fact]
        public void Can_Split_String_Without_TextQualifier()
        {
            // Arrange
            var input = "Value A\tValue B\tValue C";

            // Act
            var actual = input.SplitDelimited('\t');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "Value A", "Value B", "Value C" });
        }

        [Fact]
        public void Can_Split_String_With_TextQualifier()
        {
            // Arrange
            var input = "\"Value A\" \"Value B\" \"Value C\"";

            // Act
            var actual = input.SplitDelimited(' ', '"');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "Value A", "Value B", "Value C" });
        }

        [Fact]
        public void Can_Split_String_With_TextQualifier_But_Not_Present()
        {
            // Arrange
            var input = "Value A Value B Value C";

            // Act
            var actual = input.SplitDelimited(' ', '"');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "Value", "A", "Value", "B", "Value", "C" });
        }

        [Fact]
        public void Can_Split_String_With_TextQualifier_Using_Delimiter_At_The_End()
        {
            // Arrange
            var input = "a,\"b,c\",";

            // Act
            var actual = input.SplitDelimited(',', '"');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "a", "b,c", string.Empty });
        }

        [Fact]
        public void Can_Split_String_Without_TextQualifier_Using_Delimiter_At_The_End()
        {
            // Arrange
            var input = "Value A,Value B,Value C,";

            // Act
            var actual = input.SplitDelimited(',');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "Value A", "Value B", "Value C", string.Empty });
        }

        [Fact]
        public void Can_Split_String_Where_Delimiter_Is_Within_TextQualifiers()
        {
            // Arrange
            var input = "\"A+B+C\"";

            // Act
            var actual = input.SplitDelimited('+', '"');

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "A+B+C" });
        }

        [Fact]
        public void Can_Split_String_With_Textqualifier_And_Leave_The_TextQualifier()
        {
            // Arrange
            var input = "a,\"b,c\"";

            // Act
            var actual = input.SplitDelimited(',', '"', leaveTextQualifier: true);

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "a", "\"b,c\"" });
        }

        [Fact]
        public void Can_Split_String_And_Trim_Each_Item()
        {
            // Arrange
            var input = "Value A \t Value B \t Value C";

            // Act
            var actual = input.SplitDelimited('\t', trimItems: true);

            // Assert
            actual.ShouldBeEquivalentTo(new[] { "Value A", "Value B", "Value C" });
        }

        [Fact]
        public void Can_Not_Split_String_With_Null_Delimiter()
        {
            // Arrange
            var input = "Value A \t Value B \t Value C";

            // Act
            Action a = () => input.SplitDelimited((string)null!);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Can_Not_Split_String_With_Three_Character_Delimiter()
        {
            // Arrange
            var input = "Value A \t Value B \t Value C";

            // Act
            Action a = () => input.SplitDelimited("===");
            a.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Can_Split_String_With_Two_Character_Delimiter_And_Trim_Items()
        {
            // Arrange
            var input = "\"My value with ==\" == \"My other value with ==\"";

            // Act
            var result = input.SplitDelimited("==", '\"', true, trimItems: true);
            // Assert
            result.ShouldBeEquivalentTo(new[] { "\"My value with ==\"", "\"My other value with ==\"" });
        }

        [Fact]
        public void Can_Split_String_With_Two_Character_Delimiter_No_Trimmed_Items()
        {
            // Arrange
            var input = "\"My value with ==\" == \"My other value with ==\"";

            // Act
            var result = input.SplitDelimited("==", '\"', true, trimItems: false);

            // Assert
            result.ShouldBeEquivalentTo(new[] { "\"My value with ==\" ", " \"My other value with ==\"" });
        }

        [Fact]
        public void Can_Split_String_With_Two_Character_Delimiter_And_String_Ends_With_Delimiter()
        {
            // Arrange
            var input = "\"My value with ==\" == \"My other value with ==\" =";

            // Act
            var result = input.SplitDelimited("==", '\"', true, trimItems: true);
            // Assert
            result.ShouldBeEquivalentTo(new[] { "\"My value with ==\"", "\"My other value with ==\" =" });
        }

        [Fact]
        public void Can_Split_String_With_Delimiter_Array()
        {
            // Arrange
            var input = "A,B,C,D";

            // Act
            var result = input.SplitDelimited([","]).ToArray();

            // Assert
            result.ShouldBeEquivalentTo(new[] { "A", "B", "C", "D" });
        }

        [Fact]
        public void Can_Split_String_With_Delimiter_Array_Using_Text_Qualifier()
        {
            // Arrange
            var input = "A,\"B,C\",D";

            // Act
            var result = input.SplitDelimited([","], textQualifier: '"').ToArray();

            // Assert
            result.ShouldBeEquivalentTo(new[] { "A", "B,C", "D" });
        }
        [Fact]
        public void Can_Split_String_With_AND_and_OR_Delimiters()
        {
            // Arrange
            var input = "My value AND \"some value with ignored AND\" OR some other value";

            // Act
            var result = input.SplitDelimited(["AND", "OR"], '"', true, true).ToArray();

            // Assert
            result.ShouldBeEquivalentTo(new[] { "My value", @"""some value with ignored AND""", "some other value" });
        }

        [Fact]
        public void Can_Split_String_With_Mathematical_Delimiters()
        {
            // Arrange
            var input = "1 + 2 / 4 * 8 - 1";

            // Act
            var result = input.SplitDelimited(["+", "-", "/", "*"], trimItems: true).ToArray();

            // Assert
            result.ShouldBeEquivalentTo(new[] { "1", "2", "4", "8", "1" });
        }

        [Fact]
        public void Empty_String_Results_In_Empty_Array()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var actual = input.SplitDelimited(',');

            // Assert
            actual.ShouldBeEmpty();
        }

        [Fact]
        public void Empty_String_Results_In_Empty_Array_When_Using_Array_Of_Delimiters()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var actual = input.SplitDelimited([","]);

            // Assert
            actual.ShouldBeEmpty();
        }
    }
}
