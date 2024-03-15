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
        results.Single().ErrorMessage.Should().Be("The field MyProperty must be a collection type with a maximum count of '10'.");
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
        results.Single().ErrorMessage.Should().Be("The field MyProperty must be a collection type with a maximum count of '10'.");
    }

    [Fact]
    public void Is_Valid_When_MaximumCount_Is_Unrestricted()
    {
        // Arrange
        var sut = new MyClassWithUnrestrictedCount { MyProperty = new List<int> { 1, 2, 3 } };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Throws_When_MaximumCount_Is_Zero()
    {
        // Arrange
        var sut = new MyClassWithCountZero();
        var results = new List<ValidationResult>();

        // Act & Assert
        sut.Invoking(x => _ = x.TryValidate(results))
           .Should().Throw<InvalidOperationException>().WithMessage("MaxCountAttribute must have a Count value that is greater than zero. Use MaxCount() without parameters to indicate that the collection can have the maximum allowable count.");
    }

    [Fact]
    public void Throws_When_MaximumCount_Is_Lower_Than_MinusOne()
    {
        // Arrange
        var sut = new MyClassWithCountMinusTwo();
        var results = new List<ValidationResult>();

        // Act & Assert
        sut.Invoking(x => _ = x.TryValidate(results))
           .Should().Throw<InvalidOperationException>().WithMessage("MaxCountAttribute must have a Count value that is greater than zero. Use MaxCount() without parameters to indicate that the collection can have the maximum allowable count.");
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

    private sealed class MyClassWithUnrestrictedCount
    {
        [MaxCount]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyClassWithCountZero
    {
        [MaxCount(0)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyClassWithCountMinusTwo
    {
        [MaxCount(-2)]
        public List<int> MyProperty { get; set; } = default!;
    }
}
