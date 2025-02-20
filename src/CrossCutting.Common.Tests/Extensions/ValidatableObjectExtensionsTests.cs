namespace CrossCutting.Common.Tests.Extensions;

public class ValidatableObjectExtensionsTests
{
    [Fact]
    public void Validate_ResultsOverload_Returns_True_When_Valid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = "Filled" };
        var results = new List<ValidationResult>();

        // Act
        var actual = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), new[] { new RequiredAttribute() });

        // Assert
        actual.ShouldBe(true);
        results.Count.ShouldBe(0);
    }

    [Fact]
    public void Validate_ResultsOverload_Returns_False_When_Invalid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = null };
        var results = new List<ValidationResult>();

        // Act
        var actual = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), new[] { new RequiredAttribute() });

        // Assert
        actual.ShouldBe(false);
        results.Count.ShouldBe(1);
    }

    [Fact]
    public void Validate_ResultsOverload_Returns_Null_When_ValidationAttributes_Is_Null_Or_Empty()
    {
        // Arrange
        var input = new MyValidatableClass { Value = null };
        var results = new List<ValidationResult>();

        // Act
        var actual_null = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), null);
        var actual_empty = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), Enumerable.Empty<RequiredAttribute>());

        // Assert
        actual_null.ShouldBeNull();
        actual_empty.ShouldBeNull();
    }

    [Fact]
    public void Validate_MemberNameOverload_Returns_Empty_When_Valid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = "Filled" };

        // Act
        var actual = input.Validate(nameof(MyValidatableClass.Value));

        // Assert
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void Validate_MemberNameOverload_Returns_ErrorMessage_When_Invalid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = null };

        // Act
        var actual = input.Validate(nameof(MyValidatableClass.Value));

        // Assert
        actual.ShouldBe("Value is required");
    }

    [Fact]
    public void Validate_NoArgumentsOverload_Returns_Empty_When_Valid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = "Filled" };

        // Act
        var actual = input.Validate();

        // Assert
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void Validate_NoArgumentsOverload_Returns_ErrorMessage_When_Invalid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = null };

        // Act
        var actual = input.Validate();

        // Assert
        actual.ShouldBe("Value is required");
    }

    [Fact]
    public void Validate_NoArgumentsOverload_Returns_ErrorMessage_When_Invalid_With_Two_Errors()
    {
        // Arrange
        var input = new MyValidatableClassWithTwoProperties { Value1 = null, Value2 = null };

        // Act
        var actual = input.Validate();

        // Assert
        actual.ShouldContain("Value1 is required");
        actual.ShouldContain("Value2 is required");
    }

    [Fact]
    public void Validate_ExceptionOverload_Does_Not_Throw_When_Valid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = "Filled" };

        // Act & Assert
        Action a = () => input.Validate<ValidationException>();
        a.ShouldNotThrow();
    }

    [Fact]
    public void Validate_ExceptionOverload_Throws_When_Invalid()
    {
        // Arrange
        var input = new MyValidatableClass { Value = null };

        // Act & Assert
        Action a = () => input.Validate<ValidationException>();
        a.ShouldThrow<ValidationException>();
    }

    private sealed class MyValidatableClass : IValidatableObject
    {
        public string? Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Value))
            {
                yield return new ValidationResult("Value is required", [nameof(Value)]);
            }
        }
    }

    private sealed class MyValidatableClassWithTwoProperties : IValidatableObject
    {
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Value1))
            {
                yield return new ValidationResult("Value1 is required", [nameof(Value1)]);
            }
            if (string.IsNullOrEmpty(Value2))
            {
                yield return new ValidationResult("Value2 is required", [nameof(Value2)]);
            }
        }
    }
}
