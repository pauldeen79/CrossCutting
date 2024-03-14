namespace CrossCutting.Common.Tests.DataAnnotations;

public class MinCountAttributeTests
{
    [Fact]
    public void Is_Valid_On_Null_Value()
    {
        // Arrange
        var sut = new MyClass { MyProperty = null! };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Is_Valid_When_Value_Falls_Within_Range()
    {
        // Arrange
        var sut = new MyClass { MyProperty = new List<int> { 1, 2, 3 } };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Is_Invalid_When_Value_Is_Lower_Than_Minimum()
    {
        // Arrange
        var sut = new MyClass { MyProperty = new List<int>() };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Should().ContainSingle();
    }

    [Fact]
    public void Is_Invalid_When_Value_Does_Not_Implement_IList()
    {
        // Arrange
        var sut = new MyWrongTypeClass { MyProperty = 11 };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Should().ContainSingle();
    }

    private sealed class MyClass
    {
        [MinCount(1)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyWrongTypeClass
    {
        [MinCount(1)]
        public int MyProperty { get; set; }
    }
}
