namespace CrossCutting.Common.Tests.DataAnnotations;

public class ValidateObjectAttributeTests
{
    [Fact]
    public void Can_Validate_Recursively()
    {
        // Arrange
        var sut = new MyValidatableClass { SubProperty = new MyValidatableClass() };
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "SubProperty: The Name field is required.");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "SubProperty");
    }

    [Fact]
    public void Can_Validate_Collection()
    {
        // Arrange
        var sut = new MyValidatableClass();
        sut.CollectionSubProperty.Add(new MyValidatableClass());
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "CollectionSubProperty: The Name field is required.");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "CollectionSubProperty");
    }
}

public class MyValidatableClass
{
    [Required] public string Name { get; set; } = "";

    [ValidateObject] public MyValidatableClass? SubProperty { get; set; }
    [ValidateObject] public Collection<MyValidatableClass> CollectionSubProperty { get; } = new();
}
