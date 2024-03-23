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
    public void ToStringWithNullCheck_Returns_Correct_Result(string? input, string expectedOutput)
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

    [Fact]
    public void ToString_Returns_DefaultValue_When_Input_Is_Null()
    {
        // Arrange
        object? input = null;

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.Should().Be("default value");
    }

    [Fact]
    public void ToString_Returns_Formatted_Value_When_Input_Is_IFormattable()
    {
        // Arrange
        object? input = 6.5M;

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.Should().Be("6.5");
    }

    [Fact]
    public void ToString_Returns_ToString_Representation_When_Input_Is_Not_IFormattable()
    {
        // Arrange
        object? input = "some string value";

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.Should().Be("some string value");
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
    public void IsTrue_Returns_StringValue_Correctly(string? input, bool expectedOutput)
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
    public void IsFalse_Returns_StringValue_Correctly(string? input, bool expectedOutput)
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
    public void In_Returns_False_When_Instance_Is_Null_ParamArray()
    {
        // Arrange
        string? instance = default;

        // Act
        var result = instance.In("A", "B", "C");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void In_Returns_False_When_Instance_Is_Null_Enumerable()
    {
        // Arrange
        string? instance = default;

        // Act
        var result = instance.In(new List<string> { "A", "B", "C" });

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void In_Throws_On_Null_Values_ParamArray()
    {
        // Arrange
        string instance = "A";
        string[] values = default!;

        // Act
        instance.Invoking(x => _ = x.In(values))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("values");
    }

    [Fact]
    public void In_Throws_On_Null_Values_Enumerable()
    {
        // Arrange
        string instance = "A";
        IEnumerable<string> values = default!;

        // Act
        instance.Invoking(x => _ = x.In(values))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("values");
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

    [Fact]
    public void Can_Convert_Object_To_Result_Success()
    {
        // Arrange
        MyPocoClass? sut = new();

        // Act
        var actual = sut.ToResult();

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.Value.Should().BeSameAs(sut);
    }

    [Fact]
    public void Can_Convert_Object_To_Result_NotFound_Without_ErrorMessage()
    {
        // Arrange
        MyPocoClass? sut = default;

        // Act
        var actual = sut.ToResult();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.Value.Should().BeNull();
        actual.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Can_Convert_Object_To_Result_NotFound_With_ErrorMessage()
    {
        // Arrange
        MyPocoClass? sut = default;

        // Act
        var actual = sut.ToResult("My error message");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.Value.Should().BeNull();
        actual.ErrorMessage.Should().Be("My error message");
    }

    [Fact]
    public void Can_Transform_Object_Using_ExtensionMethod()
    {
        // Arrange
        var sut = true;

        // Act
        var actual = sut.Transform(x => x ? "true" : "false");

        // Assert
        actual.Should().Be("true");
    }

    [Fact]
    public void Can_Perform_Operation_On_All_Properties_Non_Lazy()
    {
        // Arrange
        var sut = new MyPocoClassWithList();
        sut.Items.Add(new MyPocoClass { Value = "item1" });
        sut.Items.Add(new MyPocoClass { Value = "item2" });

        // Act
        var actual = sut.WithAll(sut.Items, x => x.Value = x.Value!.ToUpper(CultureInfo.CurrentCulture));

        // Assert
        actual.Items.Select(x => x.Value).Should().BeEquivalentTo("ITEM1", "ITEM2");
    }

    [Fact]
    public void Can_Perform_Operation_On_All_Properties_Lazy()
    {
        // Arrange
        var sut = new MyPocoClassWithList();
        sut.Items.Add(new MyPocoClass { Value = "item1" });
        sut.Items.Add(new MyPocoClass { Value = "item2" });

        // Act
        var actual = sut.WithAll(x => x.Items, x => x.Value = x.Value!.ToUpper(CultureInfo.CurrentCulture));

        // Assert
        actual.Items.Select(x => x.Value).Should().BeEquivalentTo("ITEM1", "ITEM2");
    }

    private sealed class MyPocoClass
    {
        [Required]
        public string? Value { get; set; }
    }

    private sealed class MyPocoClassWithList
    {
        public List<MyPocoClass> Items { get; set; } = new();
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
