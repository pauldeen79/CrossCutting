namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class StringExtensionsTests
{
    public class WithoutGenerics : StringExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Value_Without_GenericArguments()
        {
            // Arrange
            const string input = "SomeValue";

            // Act
            var result = input.WithoutGenerics();

            // Assert
            result.ShouldBe(input);
        }

        [Fact]
        public void Returns_Correct_Result_On_Value_With_GenericArguments()
        {
            // Arrange
            const string input = "MyType<MyGenericArgument>";

            // Act
            var result = input.WithoutGenerics();

            // Assert
            result.ShouldBe("MyType");
        }
    }

    public class GetGenericArguments : StringExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Value_Without_GenericArguments()
        {
            // Arrange
            const string input = "SomeValue";

            // Act
            var result = input.GetGenericArguments();

            // Assert
            result.ShouldBeEmpty(); ;
        }

        [Fact]
        public void Returns_Correct_Result_On_Value_With_GenericArguments()
        {
            // Arrange
            const string input = "MyType<MyGenericArgument>";

            // Act
            var result = input.GetGenericArguments();

            // Assert
            result.ShouldBe("MyGenericArgument");
        }
    }
}
