namespace CrossCutting.Common.Tests;

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
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();

        // Act
        var actual = new ValueCollection<string>(equalityComparerMock);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_Prefilled_ValueCollection_With_Default_EqualityComparer()
    {
        // Act
        var actual = new ValueCollection<string>(new[] { "a", "b", "c" });

        // Assert
        actual.Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void Can_Construct_Prefilled_ValueCollection_With_Custom_EqualityComparer()
    {
        // Arrange
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();
        equalityComparerMock.Equals(Arg.Any<string>(), Arg.Any<string>())
                            .Returns(x => x.ArgAt<string>(0).ToUpperInvariant() == x.ArgAt<string>(1).ToUpperInvariant());

        // Act
        var actual = new ValueCollection<string>(new[] { "a", "b", "c" }, equalityComparerMock);

        // Assert
        actual.Should().BeEquivalentTo("a", "b", "c");
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
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();
        equalityComparerMock.Equals(Arg.Any<string>(), Arg.Any<string>())
                            .Returns(x => x.ArgAt<string>(0).ToUpperInvariant() == x.ArgAt<string>(1).ToUpperInvariant());
        var value1 = new ValueCollection<string>(new[] { "a", "b", "C" }, equalityComparerMock);
        var value2 = new ValueCollection<string>(new[] { "A", "b", "c" }, equalityComparerMock);

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
        var value = new ValueCollection<bool>(new[] { true, false });

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
        var value = new ValueCollection<DateTime>(new[] { new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Unspecified) });

        // Act
        var actual = value.ToString(null, CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be("[2020-02-01T00:00:00.0000000]");
    }
}
