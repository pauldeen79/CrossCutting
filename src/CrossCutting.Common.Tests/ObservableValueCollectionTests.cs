namespace CrossCutting.Common.Tests;

public class ObservableValueCollectionTests
{
    [Fact]
    public void Can_Construct_Empty_ValueCollection_With_Default_EqualityComparer()
    {
        // Act
        var actual = new ObservableValueCollection<string>();

        // Assert
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Construct_Empty_ValueCollection_With_Custom_EqualityComparer()
    {
        // Arrange
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();

        // Act
        var actual = new ObservableValueCollection<string>(equalityComparerMock);

        // Assert
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Construct_Prefilled_ValueCollection_With_Default_EqualityComparer()
    {
        // Act
        var actual = new ObservableValueCollection<string>(["a", "b", "c"]);

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Can_Construct_Prefilled_ValueCollection_With_Custom_EqualityComparer()
    {
        // Arrange
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();
        equalityComparerMock.Equals(Arg.Any<string>(), Arg.Any<string>())
                            .Returns(x => x.ArgAt<string>(0).ToUpperInvariant() == x.ArgAt<string>(1).ToUpperInvariant());

        // Act
        var actual = new ObservableValueCollection<string>(["a", "b", "c"], equalityComparerMock);

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Can_Compare_Two_Value_Equal_ObservableCollections_With_Default_EqualityComparer()
    {
        // Arrange
        var value1 = new ObservableValueCollection<string>(["a", "b", "c"]);
        var value2 = new ObservableValueCollection<string>(["a", "b", "c"]);

        // Act
        var actual = value1.Equals(value2);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Can_Compare_Two_Different_ObservableCollections_With_Default_EqualityComparer_1()
    {
        // Arrange
        var value1 = new ObservableValueCollection<string>(["a", "b", "c"]);
        var value2 = new ObservableValueCollection<string>(["a", "b"]);

        // Act
        var actual = value1.Equals(value2);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Can_Compare_Two_Different_ObservableCollections_With_Default_EqualityComparer_2()
    {
        // Arrange
        var value1 = new ObservableValueCollection<string>(["a", "b"]);
        var value2 = new ObservableValueCollection<string>(["a", "b", "c"]);

        // Act
        var actual = value1.Equals(value2);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Can_Compare_Two_ObservableCollections_With_Custom_EqualityComparer()
    {
        // Arrange
        var equalityComparerMock = Substitute.For<IEqualityComparer<string>>();
        equalityComparerMock.Equals(Arg.Any<string>(), Arg.Any<string>())
                            .Returns(x => x.ArgAt<string>(0).ToUpperInvariant() == x.ArgAt<string>(1).ToUpperInvariant());
        var value1 = new ObservableValueCollection<string>(["a", "b", "C"], equalityComparerMock);
        var value2 = new ObservableValueCollection<string>(["A", "b", "c"], equalityComparerMock);

        // Act
        var actual = value1.Equals(value2);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Equals_Returns_False_When_Other_Is_Null()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b", "C"]);

        // Act
        var actual = value.Equals(null);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Equals_Returns_True_On_Same_Reference()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b", "C"]);

        // Act
        var actual = value.Equals(value);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_Two_ObservableCollections_Returns_True_When_Value_Equal()
    {
        // Arrange
        var value1 = new ObservableValueCollection<string>(["a", "b", "c"]);
        var value2 = new ObservableValueCollection<string>(["a", "b", "c"]);

        // Act
        var actual = value1.Equals((object)value2);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_Returns_False_When_Other_Is_Null()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b", "C"]);

        // Act
        var actual = value.Equals((object?)null);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_Returns_True_On_Same_Reference()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b", "C"]);

        // Act
        var actual = value.Equals((object)value);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_Different_Other_Type_Returns_False()
    {
        // Arrange
        var value1 = new ObservableValueCollection<string>(["a", "b", "c"]);
        var value2 = "wrong type";

        // Act
        var actual = value1.Equals((object)value2);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void ToString_Gives_Correct_Result_On_Null_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<string?>([null, null]);

        // Act
        var actual = value.ToString();

        // Assert
        actual.ShouldBe("[∅, ∅]");
    }

    [Fact]
    public void ToString_Gives_Correct_Result_On_Bool_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<bool>([true, false]);

        // Act
        var actual = value.ToString();

        // Assert
        actual.ShouldBe("[true, false]");
    }

    [Fact]
    public void ToString_Gives_Correct_Result_On_String_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b"]);

        // Act
        var actual = value.ToString();

        // Assert
        actual.ShouldBe(@"[""a"", ""b""]");
    }

    [Fact]
    public void ToString_Gives_Correct_Result_On_Char_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<char>(['a', 'b']);

        // Act
        var actual = value.ToString();

        // Assert
        actual.ShouldBe("['a', 'b']");
    }

    [Fact]
    public void ToString_Gives_Correct_Result_On_DateTime_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<DateTime>([new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Unspecified)]);

        // Act
        var actual = value.ToString(null, CultureInfo.InvariantCulture);

        // Assert
        actual.ShouldBe("[2020-02-01T00:00:00.0000000]");
    }

    [Fact]
    public void GetHashCode_Returns_Value()
    {
        // Arrange
        var value = new ObservableValueCollection<string>(["a", "b", "C"]);

        // Act
        var actual = value.GetHashCode();

        // Assert
        actual.ShouldNotBe(default);
    }
}
