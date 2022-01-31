namespace CrossCutting.Common.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void Can_Validate_Poco_Object()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act & Assert
        input.Invoking(x => x.Validate())
             .Should().Throw<ValidationException>();
    }

    [Fact]
    public void Can_TryValidate_Poco_Object()
    {
        // Arrange
        var input = new MyPocoClass();
        var validationResults = new Collection<ValidationResult>();

        // Act
        var actual = input.TryValidate(validationResults);

        // Assert
        actual.Should().BeFalse();
        validationResults.Should().HaveCount(1);
    }

    [Fact]
    public void Can_Call_TryDispose_On_NonDisposable_Object()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act & assert
        input.Invoking(x => x.TryDispose())
             .Should().NotThrow();
    }

    [Fact]
    public void Can_Call_TryDispose_On_Disposable_Object()
    {
        // Arrange
        using var input = new MyDisposableClass();

        // Act
        input.TryDispose();

        // Assert
        input.IsDisposed.Should().BeTrue();
    }

    [Theory,
        InlineData("", ""),
        InlineData(null, ""),
        InlineData("a", "a")]
    public void ToStringWithNullCheck_Returns_Correct_Result(string input, string expectedOutput)
    {
        // Act
        var actual = input.ToStringWithNullCheck();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Fact]
    public void ToStringWithDefault_Returns_DefaultValue_On_Null_Input()
    {
        // Act
        var actual = ((object?)null).ToStringWithDefault(() => "default");

        // Assert
        actual.Should().Be("default");
    }

    [Fact]
    public void ToStringWithDefault_Returns_StringEmpty_On_Null_Input_When_DefaultValue_Is_Not_Supplied()
    {
        // Act
        var actual = ((object?)null).ToStringWithDefault();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ToStringWithDefault_Returns_Input_On_NonNull_Input()
    {
        // Act
        var actual = "non null value".ToStringWithDefault("default");

        // Assert
        actual.Should().Be("non null value");
    }

    [Theory,
        InlineData(true),
        InlineData(false)]
    public void IsTrue_Returns_Boolean_Value_Unchanged(bool input)
    {
        // Act
        var actual = input.IsTrue();

        // Assert
        actual.Should().Be(input);
    }

    [Theory,
        InlineData("True", true),
        InlineData("False", false),
        InlineData("", false),
        InlineData(null, false)]
    public void IsTrue_Returns_StringValue_Correctly(string input, bool expectedOutput)
    {
        // Act
        var actual = input.IsTrue();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory,
        InlineData(true),
        InlineData(false)]
    public void IsFalse_Returns_Reversed_Boolean_Value(bool input)
    {
        // Act
        var actual = input.IsFalse();

        // Assert
        actual.Should().Be(!input);
    }

    [Theory,
        InlineData("True", false),
        InlineData("False", true),
        InlineData("", false),
        InlineData(null, false)]
    public void IsFalse_Returns_StringValue_Correctly(string input, bool expectedOutput)
    {
        // Act
        var actual = input.IsFalse();

        // Assert
        actual.Should().Be(expectedOutput);
    }
    [Fact]
    public void IsTrue_With_Predicate_Returns_Correct_Result()
    {
        // Arrange
        var input = new MyPocoClass { Value = "A" };

        // Act
        var actual_true = input.IsTrue(x => x.Value == "A");
        var actual_false = input.IsTrue(x => x.Value != "A");

        // Assert
        actual_true.Should().BeTrue();
        actual_false.Should().BeFalse();
    }

    [Fact]
    public void IsFalse_With_Predicate_Returns_Correct_Result()
    {
        // Arrange
        var input = new MyPocoClass { Value = "A" };

        // Act
        var actual_true = input.IsFalse(x => x.Value == "A");
        var actual_false = input.IsFalse(x => x.Value != "A");

        // Assert
        actual_true.Should().BeFalse();
        actual_false.Should().BeTrue();
    }

    [Fact]
    public void In_Returns_True_When_Array_Of_Arguments_Contains_Correct_Value()
    {
        // Arrange
        var input = "A";

        // Act
        var actual = input.In("A", "B", "C");

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void In_Returns_False_When_Array_Of_Arguments_Does_Not_Contain_Correct_Value()
    {
        // Arrange
        var input = "D";

        // Act
        var actual = input.In("A", "B", "C");

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void In_Returns_True_When_Enumerable_Of_Arguments_Contains_Correct_Value()
    {
        // Arrange
        var input = "A";

        // Act
        var actual = input.In(new List<string> { "A", "B", "C" });

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void In_Returns_False_When_Enumerable_Of_Arguments_Does_Not_Contain_Correct_Value()
    {
        // Arrange
        var input = "D";

        // Act
        var actual = input.In(new List<string> { "A", "B", "C" });

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Can_Convert_Class_To_ExpandoObject()
    {
        // Arrange
        var input = new MyPocoClass { Value = "MyValue" };

        // Act
        var actual = input.ToExpandoObject();

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Key.Should().Be("Value");
        actual.First().Value.Should().Be("MyValue");
    }

    [Fact]
    public void Can_Convert_Enumerable_Of_KeyValuePair_To_ExpandoObject()
    {
        // Arrange
        var input = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Value", "MyValue")
            };

        // Act
        var actual = input.ToExpandoObject();

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Key.Should().Be("Value");
        actual.First().Value.Should().Be("MyValue");
    }

    [Fact]
    public void Can_Chain_Without_Argument()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act
        var actual = input.Chain(() => { /* something interesting */ });

        // Assert
        actual.Should().BeSameAs(input);
    }

    [Fact]
    public void Can_Chain_With_Argument()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act
        var actual = input.Chain(_ => { /* something interesting */ });

        // Assert
        actual.Should().BeSameAs(input);
    }

    private class MyPocoClass
    {
        [Required]
        public string? Value { get; set; }
    }

    private sealed class MyDisposableClass : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
