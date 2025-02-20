namespace CrossCutting.Common.Tests.DataAnnotations;

public class CountAttributeTests
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
        results.ShouldBeEmpty();
    }

    [Fact]
    public void Is_Valid_When_Value_Falls_Within_Range()
    {
        // Arrange
        var sut = new MyClass { MyProperty = [1, 2, 3] };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.ShouldBeEmpty();
    }

    [Fact]
    public void Is_Invalid_When_Value_Is_Lower_Than_Minimum()
    {
        // Arrange
        var sut = new MyClass { MyProperty = [] };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Count.ShouldBe(1);
        results.Single().ErrorMessage.ShouldBe("The field MyProperty must be a collection with a minimum count of 1 and a maximum count of 10.");
    }

    [Fact]
    public void Is_Invalid_When_Value_Is_Higher_Than_Maximum()
    {
        // Arrange
        var sut = new MyClass { MyProperty = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Count.ShouldBe(1);
        results.Single().ErrorMessage.ShouldBe("The field MyProperty must be a collection with a minimum count of 1 and a maximum count of 10.");
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
        results.Count.ShouldBe(1);
        results.Single().ErrorMessage.ShouldBe("The field MyProperty must be a collection with a minimum count of 1 and a maximum count of 10.");
    }

    [Fact]
    public void Throws_When_MaximumCount_Is_Lower_Than_Zero()
    {
        // Arrange
        var sut = new MyClassWithMaximumCountLowerThanZero();
        var results = new List<ValidationResult>();

        // Act & Assert
        Action a = () => _ = sut.TryValidate(results);
        a.ShouldThrow<InvalidOperationException>().Message.ShouldBe("The maximum count must be a nonnegative integer.");
    }

    [Fact]
    public void Throws_When_MaximumCount_Is_Lower_Than_MinimumCount()
    {
        // Arrange
        var sut = new MyClassWithMaximumCountLowerThanMinimumCount();
        var results = new List<ValidationResult>();

        // Act & Assert
        Action a = () => sut.TryValidate(results);
        a.ShouldThrow<InvalidOperationException>().Message.ShouldBe("The maximum value '1' must be greater than or equal to the minimum value '10'.");
    }

    private sealed class MyClass
    {
        [Count(1, 10)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyWrongTypeClass
    {
        [Count(1, 10)]
        public int MyProperty { get; set; }
    }

    private sealed class MyClassWithMaximumCountLowerThanZero
    {
        [Count(1, -1)]
        public List<int> MyProperty { get; set; } = default!;
    }

    private sealed class MyClassWithMaximumCountLowerThanMinimumCount
    {
        [Count(10, 1)]
        public List<int> MyProperty { get; set; } = default!;
    }
}
