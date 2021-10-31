using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class ValueCollectionTests
    {
        [Fact]
        public void Can_Construct_Empty_ValueCollection_With_Default_EqualityComparer()
        {
            // Act
            var actual = new ValueCollection<string>();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_Empty_ValueCollection_With_Custom_EqualityComparer()
        {
            // Arrange
            var equalityComparerMock = new Mock<IEqualityComparer<string>>();

            // Act
            var actual = new ValueCollection<string>(equalityComparerMock.Object);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_Prefilled_ValueCollection_With_Default_EqualityComparer()
        {
            // Act
            var actual = new ValueCollection<string>(new[] { "a", "b", "c" });

            // Assert
            actual.Should().BeEquivalentTo(new[] { "a", "b", "c" });
        }

        [Fact]
        public void Can_Construct_Prefilled_ValueCollection_With_Custom_EqualityComparer()
        {
            // Arrange
            var equalityComparerMock = new Mock<IEqualityComparer<string>>();
            equalityComparerMock.Setup(x => x.Equals(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns<string, string>((x, y) => x.ToUpperInvariant() == y.ToUpperInvariant());

            // Act
            var actual = new ValueCollection<string>(new[] { "a", "b", "c" }, equalityComparerMock.Object);

            // Assert
            actual.Should().BeEquivalentTo(new[] { "a", "b", "c" });
        }

        [Fact]
        public void Can_Compare_Two_Value_Equal_ValueCollections_With_Default_EqualityComparer()
        {
            // Arrange
            var value1 = new ValueCollection<string>(new[] { "a", "b", "c" });
            var value2 = new ValueCollection<string>(new[] { "a", "b", "c" });

            // Act
            var actual = value1.Equals(value2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void Can_Compare_Two_Different_ValueCollections_With_Default_EqualityComparer_1()
        {
            // Arrange
            var value1 = new ValueCollection<string>(new[] { "a", "b", "c" });
            var value2 = new ValueCollection<string>(new[] { "a", "b" });

            // Act
            var actual = value1.Equals(value2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void Can_Compare_Two_Different_ValueCollections_With_Default_EqualityComparer_2()
        {
            // Arrange
            var value1 = new ValueCollection<string>(new[] { "a", "b" });
            var value2 = new ValueCollection<string>(new[] { "a", "b", "c" });

            // Act
            var actual = value1.Equals(value2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void Can_Compare_Two_ValueCollections_With_Custom_EqualityComparer()
        {
            // Arrange
            var equalityComparerMock = new Mock<IEqualityComparer<string>>();
            equalityComparerMock.Setup(x => x.Equals(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns<string, string>((x, y) => x.ToUpperInvariant() == y.ToUpperInvariant());
            var value1 = new ValueCollection<string>(new[] { "a", "b", "C" }, equalityComparerMock.Object);
            var value2 = new ValueCollection<string>(new[] { "A", "b", "c" }, equalityComparerMock.Object);

            // Act
            var actual = value1.Equals(value2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void Equals_Returns_False_When_Other_Is_Null()
        {
            // Arrange
            var value = new ValueCollection<string>(new[] { "a", "b", "C" });

            // Act
            var actual = value.Equals(null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void Equals_Returns_True_On_Same_Reference()
        {
            // Arrange
            var value = new ValueCollection<string>(new[] { "a", "b", "C" });

            // Act
            var actual = value.Equals(value);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void Equals_Object_Two_ValueCollections_Returns_True_When_Value_Equal()
        {
            // Arrange
            var value1 = new ValueCollection<string>(new[] { "a", "b", "c" });
            var value2 = new ValueCollection<string>(new[] { "a", "b", "c" });

            // Act
            var actual = value1.Equals((object)value2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void Equals_Object_Returns_False_When_Other_Is_Null()
        {
            // Arrange
            var value = new ValueCollection<string>(new[] { "a", "b", "C" });

            // Act
            var actual = value.Equals((object?)null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void Equals_Object_Returns_True_On_Same_Reference()
        {
            // Arrange
            var value = new ValueCollection<string>(new[] { "a", "b", "C" });

            // Act
            var actual = value.Equals((object)value);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void ToString_Gives_Correct_Result_On_Null_Value()
        {
            // Arrange
            var value = new ValueCollection<string?>(new string?[] { null, null });

            // Act
            var actual = value.ToString();

            // Assert
            actual.Should().Be("[∅, ∅]");
        }

        [Fact]
        public void ToString_Gives_Correct_Result_On_Bool_Value()
        {
            // Arrange
            var value = new ValueCollection<bool>(new [] { true, false });

            // Act
            var actual = value.ToString();

            // Assert
            actual.Should().Be("[true, false]");
        }

        [Fact]
        public void ToString_Gives_Correct_Result_On_String_Value()
        {
            // Arrange
            var value = new ValueCollection<string>(new[] { "a", "b" });

            // Act
            var actual = value.ToString();

            // Assert
            actual.Should().Be(@"[""a"", ""b""]");
        }

        [Fact]
        public void ToString_Gives_Correct_Result_On_Char_Value()
        {
            // Arrange
            var value = new ValueCollection<char>(new[] { 'a', 'b' });

            // Act
            var actual = value.ToString();

            // Assert
            actual.Should().Be("['a', 'b']");
        }

        [Fact]
        public void ToString_Gives_Correct_Result_On_DateTime_Value()
        {
            // Arrange
            var value = new ValueCollection<DateTime>(new[] { new DateTime(2020, 2, 1) });

            // Act
            var actual = value.ToString(null, CultureInfo.InvariantCulture);

            // Assert
            actual.Should().Be("[2020-02-01T00:00:00.0000000]");
        }
    }
}
