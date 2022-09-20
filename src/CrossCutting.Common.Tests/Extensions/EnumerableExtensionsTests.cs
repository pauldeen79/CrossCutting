namespace CrossCutting.Common.Tests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Can_Use_NotNull_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.NotNull();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_NotNull_With_Predicate_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.NotNull(x => x.StartsWith("A"));

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_NotNull_With_Predicate_On_NonNull_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string> input = new[] { "A", "B", "C" };

        // Act
        var actual = input.NotNull(x => x.StartsWith("A"));

        // Assert
        actual.Should().BeEquivalentTo(new[] { "A" });
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.DefaultWhenNull();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_Null_Enumerable_With_DefaultValue_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.DefaultWhenNull(new[] { "a", "b", "c" });

        // Assert
        actual.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_NonNull_Enumerable_With_DefaultValue_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string> input = new[] { "a", "b", "c" };

        // Act
        var actual = input.DefaultWhenNull(new[] { "A", "B", "C" });

        // Assert
        actual.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void WhenEmpty_Takes_Source_When_Its_Not_Empty_Using_Enumerable()
    {
        // Arrange
        var input = new[] { "1", "2", "3" };

        // Act
        var actual = input.WhenEmpty(new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void WhenEmpty_Takes_Source_When_Its_Not_Empty_Using_Enumerable_Delegate()
    {
        // Arrange
        var input = new[] { "1", "2", "3" };

        // Act
        var actual = input.WhenEmpty(() => new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void WhenEmpty_Takes_Supplied_Value_When_Its_Not_Empty_Using_Enumerable()
    {
        // Arrange
        var input = Enumerable.Empty<string>();

        // Act
        var actual = input.WhenEmpty(new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo(new[] { "4", "5", "6" });
    }

    [Fact]
    public void WhenEmpty_Takes_Supplied_Value_When_Its_Not_Empty_Using_Enumerable_Delegate()
    {
        // Arrange
        var input = Enumerable.Empty<string>();

        // Act
        var actual = input.WhenEmpty(() => new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo(new[] { "4", "5", "6" });
    }
}
