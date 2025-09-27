namespace CrossCutting.Common.Tests.DataAnnotations;

public class AllowedTypesAttributeTests
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
    public void Is_Valid_When_Value_Is_Of_Allowed_Type()
    {
        // Arrange
        var sut = new MyClass { MyProperty = default(int) };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.ShouldBeEmpty();
    }

    [Fact]
    public void Is_Invalid_When_Value_Is_Not_Of_Allowed_Type()
    {
        // Arrange
        var sut = new MyClass { MyProperty = false };
        var results = new List<ValidationResult>();

        // Act
        _ = sut.TryValidate(results);

        // Assert
        results.Count.ShouldBe(1);
        results.Single().ErrorMessage.ShouldBe("Expression of type System.Boolean is not supported");
    }

    public class MyClass
    {
        [AllowedTypes([typeof(int), typeof(string)])]
        public object? MyProperty { get; set; }
    }
}
