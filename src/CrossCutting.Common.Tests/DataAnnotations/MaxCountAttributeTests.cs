namespace CrossCutting.Common.Tests.DataAnnotations;

public class MaxCountAttributeTests
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
    public void Is_Invalid_When_Value_Is_Higher_Than_Maximum()
    {
        // Arrange
        var sut = new MyClass { MyProperty = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 } };
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
        [MaxCount(10)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyWrongTypeClass
    {
        [MaxCount(10)]
        public int MyProperty { get; set; }
    }
}
