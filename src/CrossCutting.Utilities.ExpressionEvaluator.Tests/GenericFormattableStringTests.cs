﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class GenericFormattableStringTests
{
    public class Constructor : GenericFormattableStringTests
    {
        [Fact]
        public void Throws_On_Null_Format()
        {
            // Act & Assert
            Action a = () => _ = new GenericFormattableString(null!, ["world"]);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Should_Construct_1()
        {
            // Act & Assert
            Action a = () => _ = new GenericFormattableString("Hello {0)", ["world"]);
            a.ShouldNotThrow();
        }

        [Fact]
        public void Should_Construct_2()
        {
            // Act & Assert
            Action a = () => _ = new GenericFormattableString("Hello world");
            a.ShouldNotThrow();
        }

        [Fact]
        public void Fills_ArgumentCount_Correctly()
        {
            // Act
            var sut = new GenericFormattableString("Hello {0} {1}!", ["John", "Doe"]);

            // Assert
            sut.ArgumentCount.ShouldBe(2);
        }

        [Fact]
        public void Fills_Format_Correctly()
        {
            // Act
            var sut = new GenericFormattableString("Hello {0} {1}!", ["John", "Doe"]);

            // Assert
            sut.Format.ShouldBe("Hello {0} {1}!");
        }

        [Fact]
        public void Fills_Arguments_Correctly()
        {
            // Act
            var sut = new GenericFormattableString("Hello {0} {1}!", ["John", "Doe"]);

            // Assert
            sut.GetArguments().Length.ShouldBe(2);
        }
    }

    public class FromString : GenericFormattableStringTests
    {
        [Fact]
        public void Constructs_Correctly()
        {
            // Act
            var sut = GenericFormattableString.FromString("Hello world!");

            // Assert
            sut.Format.ShouldBe("{0}");
            sut.ArgumentCount.ShouldBe(1);
            sut.GetArgument(0).ShouldBe("Hello world!");
        }
    }

    public class ToStringMethod : GenericFormattableStringTests
    {
        [Fact]
        public void Converts_To_String_With_FormatProvider_Correctly()
        {
            // Arrange
            var sut = GenericFormattableString.FromString("Hello world!");

            // Act
            var result = sut.ToString(CultureInfo.InvariantCulture);

            // Assert
            result.ShouldBe("Hello world!");
        }

        [Fact]
        public void Converts_To_String_Without_FormatProvider_Correctly()
        {
            // Arrange
            var sut = GenericFormattableString.FromString("Hello world!");

            // Act
            var result = sut.ToString();

            // Assert
            result.ShouldBe("Hello world!");
        }
    }

    public class ImplicitOperator : GenericFormattableStringTests
    {
        [Fact]
        public void Can_Convert_Implicitly_From_String_To_GenericFormattableString()
        {
            // Arrange
            string input = "Hello world!";

            // Act
            GenericFormattableString sut = input;

            // Assert
            sut.Format.ShouldBe("{0}");
            sut.ArgumentCount.ShouldBe(1);
            sut.GetArgument(0).ShouldBe("Hello world!");
        }

        [Fact]
        public void Can_Convert_Implicitly_From_GenericFormattableString_To_String()
        {
            // Arrange
            GenericFormattableString sut = new GenericFormattableString("Hello world!");

            // Act
            string result = sut;

            // Assert
            result.ShouldBe("Hello world!");
        }
    }
}
