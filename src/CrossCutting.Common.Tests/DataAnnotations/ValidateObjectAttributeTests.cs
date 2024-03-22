namespace CrossCutting.Common.Tests.DataAnnotations;

public class ValidateObjectAttributeTests
{
    [Fact]
    public void Can_Validate_Recursively_With_Detailed_Error_Messages()
    {
        // Arrange
        var sut = new MyValidatableClassDetailed { SubProperty = new MyValidatableClassDetailed() };
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
    public void Can_Validate_Collection_With_Detailed_Error_Messages()
    {
        // Arrange
        var sut = new MyValidatableClassDetailed();
        sut.CollectionSubProperty.Add(new MyValidatableClassDetailed());
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "CollectionSubProperty: The Name field is required.");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "CollectionSubProperty");
    }

    [Fact]
    public void Can_Validate_Recursively_Without_Detailed_Error_Messages()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailed { SubProperty = new MyValidatableClassNotDetailed() };
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "The field SubProperty is invalid.");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "SubProperty");
    }

    [Fact]
    public void Can_Validate_Collection_Without_Detailed_Error_Messages()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailed();
        sut.CollectionSubProperty.Add(new MyValidatableClassNotDetailed());
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "The field CollectionSubProperty is invalid.");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "CollectionSubProperty");
    }

    [Fact]
    public void Can_Validate_Recursively_Without_Detailed_Error_Messages_Null_Value()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailed
        {
            SubProperty = null!,
            Name = "filled"
        };
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Can_Validate_Collection_Without_Detailed_Error_Messages_Null_Value()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailed
        {
            CollectionSubProperty = null!,
            Name = "filled"
        };
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Can_Validate_Recursively_Without_Detailed_Error_Messages_CustomErrorMessage()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailedCustom { SubProperty = new MyValidatableClassNotDetailedCustom() };
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
                      validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "Property SubProperty is wrong");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "SubProperty");
    }

    [Fact]
    public void Can_Validate_Collection_Without_Detailed_Error_Messages_CustomErrorMessage()
    {
        // Arrange
        var sut = new MyValidatableClassNotDetailedCustom();
        sut.CollectionSubProperty.Add(new MyValidatableClassNotDetailedCustom());
        var validationResults = new List<ValidationResult>();

        // Act
        var result = sut.TryValidate(validationResults);

        // Assert
        result.Should().BeFalse();
        validationResults.Should().HaveCount(2);
        validationResults.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "Property CollectionSubProperty is wrong");
        validationResults.SelectMany(x => x.MemberNames).Should().BeEquivalentTo("Name", "CollectionSubProperty");
    }
}

public class MyValidatableClassDetailed
{
    [Required] public string Name { get; set; } = "";

    [ValidateObject(DetailedErrorMessages = true)] public MyValidatableClassDetailed? SubProperty { get; set; }
    [ValidateObject(DetailedErrorMessages = true)] public Collection<MyValidatableClassDetailed> CollectionSubProperty { get; } = new();
}

public class MyValidatableClassNotDetailed
{
    [Required] public string Name { get; set; } = "";

    [ValidateObject] public MyValidatableClassNotDetailed? SubProperty { get; set; }
    [ValidateObject] public Collection<MyValidatableClassNotDetailed> CollectionSubProperty { get; internal set; } = new();
}

public class MyValidatableClassNotDetailedCustom
{
    [Required] public string Name { get; set; } = "";

    [ValidateObject(ErrorMessage = "Property {0} is wrong")] public MyValidatableClassNotDetailedCustom? SubProperty { get; set; }
    [ValidateObject(ErrorMessage = "Property {0} is wrong")] public Collection<MyValidatableClassNotDetailedCustom> CollectionSubProperty { get; } = new();
}
