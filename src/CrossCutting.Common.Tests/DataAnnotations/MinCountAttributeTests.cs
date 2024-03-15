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
        results.Single().ErrorMessage.Should().Be("The field MyProperty must be a collection type with a minimum length of '1'.");
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
        results.Single().ErrorMessage.Should().Be("The field MyProperty must be a collection type with a minimum length of '1'.");
    }

    [Fact]
    public void Does_Not_Throw_When_MinimumCount_Is_Zero()
    {
        // Arrange
        var sut = new MyClassWithCountZero();
        var results = new List<ValidationResult>();

        // Act & Assert
        sut.Invoking(x => _ = x.TryValidate(results))
           .Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void Throws_When_MinimumCount_Is_Lower_Than_Zero()
    {
        // Arrange
        var sut = new MyClassWithCountMinusOne();
        var results = new List<ValidationResult>();

        // Act & Assert
        sut.Invoking(x => _ = x.TryValidate(results))
           .Should().Throw<InvalidOperationException>().WithMessage("MinCountAttribute must have a Count value that is zero or greater.");
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

    private sealed class MyClassWithCountZero
    {
        [MinCount(0)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyClassWithCountMinusOne
    {
        [MinCount(-1)]
        public List<int> MyProperty { get; set; } = default!;
    }
}
