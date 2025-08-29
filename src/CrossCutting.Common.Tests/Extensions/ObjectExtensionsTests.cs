namespace CrossCutting.Common.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void Can_Validate_Poco_Object()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act & Assert
        Action a = input.Validate;
        a.ShouldThrow<ValidationException>();
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
        actual.ShouldBeFalse();
        validationResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Call_TryDispose_On_NonDisposable_Object()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act & assert
        Action a = input.TryDispose;
        a.ShouldNotThrow();
    }

    [Fact]
    public void Can_Call_TryDispose_On_Disposable_Object()
    {
        // Arrange
        using var input = new MyDisposableClass();

        // Act
        input.TryDispose();

        // Assert
        input.IsDisposed.ShouldBeTrue();
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
        actual.ShouldBe(expectedOutput);
    }

    [Fact]
    public void ToStringWithDefault_Returns_DefaultValue_On_Null_Input()
    {
        // Act
        var actual = ((object?)null).ToStringWithDefault(() => "default");

        // Assert
        actual.ShouldBe("default");
    }

    [Fact]
    public void ToStringWithDefault_Returns_StringEmpty_On_Null_Input_When_DefaultValue_Is_Not_Supplied()
    {
        // Act
        var actual = ((object?)null).ToStringWithDefault();

        // Assert
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void ToStringWithDefault_Returns_Input_On_NonNull_Input()
    {
        // Act
        var actual = "non null value".ToStringWithDefault("default");

        // Assert
        actual.ShouldBe("non null value");
    }

    [Fact]
    public void ToString_Returns_DefaultValue_When_Input_Is_Null()
    {
        // Arrange
        object? input = null;

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.ShouldBe("default value");
    }

    [Fact]
    public void ToString_Returns_Formatted_Value_When_Input_Is_IFormattable()
    {
        // Arrange
        object? input = 6.5M;

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.ShouldBe("6.5");
    }

    [Fact]
    public void ToString_Returns_ToString_Representation_When_Input_Is_Not_IFormattable()
    {
        // Arrange
        object? input = "some string value";

        // Act
        var result = input.ToString(CultureInfo.InvariantCulture, "default value");

        // Assert
        result.ShouldBe("some string value");
    }

    [Theory,
        InlineData(true),
        InlineData(false)]
    public void IsTrue_Returns_Boolean_Value_Unchanged(bool input)
    {
        // Act
        var actual = input.IsTrue();

        // Assert
        actual.ShouldBe(input);
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
        actual.ShouldBe(expectedOutput);
    }

    [Theory,
        InlineData(true),
        InlineData(false)]
    public void IsFalse_Returns_Reversed_Boolean_Value(bool input)
    {
        // Act
        var actual = input.IsFalse();

        // Assert
        actual.ShouldBe(!input);
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
        actual.ShouldBe(expectedOutput);
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
        actual_true.ShouldBeTrue();
        actual_false.ShouldBeFalse();
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
        actual_true.ShouldBeFalse();
        actual_false.ShouldBeTrue();
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Bool_True()
    {
        // Arrange
        var input = true;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Bool_False()
    {
        // Arrange
        var input = false;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_String_Null()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_String_Empty()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_String_Not_Empty()
    {
        // Arrange
        var input = "non null string";

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_ReferenceType_Null()
    {
        // Arrange
        var input = default(object?);

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_ReferenceType_Not_Null()
    {
        // Arrange
        var input = this;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Int_Zero()
    {
        // Arrange
        var input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Int_Not_Zero()
    {
        // Arrange
        var input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Long_Zero()
    {
        // Arrange
        var input = 0L;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Long_Not_Zero()
    {
        // Arrange
        var input = 1L;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Float_Zero()
    {
        // Arrange
        var input = 0f;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Float_Not_Zero()
    {
        // Arrange
        var input = 1f;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Double_Zero()
    {
        // Arrange
        var input = 0D;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Double_Not_Zero()
    {
        // Arrange
        var input = 1D;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Decimal_Zero()
    {
        // Arrange
        var input = 0M;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Decimal_Not_Zero()
    {
        // Arrange
        var input = 1M;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Signned_Byte_Zero()
    {
        // Arrange
        sbyte input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Signed_Byte_Not_Zero()
    {
        // Arrange
        sbyte input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Byte_Zero()
    {
        // Arrange
        byte input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Byte_Not_Zero()
    {
        // Arrange
        byte input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Short_Zero()
    {
        // Arrange
        short input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Shortt_Not_Zero()
    {
        // Arrange
        short input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Short_Zero()
    {
        // Arrange
        ushort input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Short_Not_Zero()
    {
        // Arrange
        ushort input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Int_Zero()
    {
        // Arrange
        uint input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Int_Not_Zero()
    {
        // Arrange
        uint input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Long_Zero()
    {
        // Arrange
        ulong input = 0;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Unsigned_Long_Not_Zero()
    {
        // Arrange
        ulong input = 1;

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Char_Zero()
    {
        // Arrange
        var input = '\0';

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public void IsTruthy_Returns_Correct_Result_For_Char_Not_Zero()
    {
        // Arrange
        var input = 'x';

        // Act
        var result = input.IsTruthy();

        // Assert
        result.ShouldBe(true);
    }

    [Fact]
    public void In_Returns_True_When_Array_Of_Arguments_Contains_Correct_Value()
    {
        // Arrange
        var input = "A";

        // Act
        var actual = input.In("A", "B", "C");

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void In_Returns_True_When_Array_Of_Arguments_Contains_Correct_Value_With_Custom_StirngComparison()
    {
        // Arrange
        var input = "a";

        // Act
        var actual = input.In(StringComparison.OrdinalIgnoreCase, "A", "B", "C");

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void In_Returns_False_When_Array_Of_Arguments_Does_Not_Contain_Correct_Value()
    {
        // Arrange
        var input = "D";

        // Act
        var actual = input.In("A", "B", "C");

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void In_Returns_True_When_Enumerable_Of_Arguments_Contains_Correct_Value()
    {
        // Arrange
        var input = "A";

        // Act
        var actual = input.In(new List<string> { "A", "B", "C" });

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void In_Returns_True_When_Enumerable_Of_Arguments_Contains_Correct_Value_With_Custom_StirngComparison()
    {
        // Arrange
        var input = "a";

        // Act
        var actual = input.In(StringComparison.OrdinalIgnoreCase, new List<string> { "A", "B", "C" });

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void In_Returns_False_When_Enumerable_Of_Arguments_Does_Not_Contain_Correct_Value()
    {
        // Arrange
        var input = "D";

        // Act
        var actual = input.In(new List<string> { "A", "B", "C" });

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void In_Returns_False_When_Instance_Is_Null_ParamArray()
    {
        // Arrange
        string? instance = default;

        // Act
        var result = instance.In("A", "B", "C");

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void In_Returns_False_When_Instance_Is_Null_Enumerable()
    {
        // Arrange
        string? instance = default;

        // Act
        var result = instance.In(new List<string> { "A", "B", "C" });

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void In_Throws_On_Null_Values_ParamArray()
    {
        // Arrange
        var instance = "A";
        string[] values = default!;

        // Act
        Action a = () => instance.In(values);
        a.ShouldThrow<ArgumentNullException>()
         .ParamName.ShouldBe("values");
    }

    [Fact]
    public void In_Throws_On_Null_Values_Enumerable()
    {
        // Arrange
        var instance = "A";
        IEnumerable<string> values = default!;

        // Act
        Action a = () => instance.In(values);
        a.ShouldThrow<ArgumentNullException>()
         .ParamName.ShouldBe("values");
    }

    [Fact]
    public void Can_Convert_Class_To_ExpandoObject()
    {
        // Arrange
        var input = new MyPocoClass { Value = "MyValue" };

        // Act
        var actual = input.ToExpandoObject();

        // Assert
        actual.Count().ShouldBe(1);
        actual.First().Key.ShouldBe("Value");
        actual.First().Value.ShouldBe("MyValue");
    }

    [Fact]
    public void Can_Convert_Enumerable_Of_KeyValuePair_To_ExpandoObject()
    {
        // Arrange
        var input = new List<KeyValuePair<string, object>>
            {
                new("Value", "MyValue")
            };

        // Act
        var actual = input.ToExpandoObject();

        // Assert
        actual.Count().ShouldBe(1);
        actual.First().Key.ShouldBe("Value");
        actual.First().Value.ShouldBe("MyValue");
    }

    [Fact]
    public void Can_Chain_Without_Argument()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act
        var actual = input.Chain(() => { /* something interesting */ });

        // Assert
        actual.ShouldBeSameAs(input);
    }

    [Fact]
    public void Can_Chain_With_Argument()
    {
        // Arrange
        var input = new MyPocoClass();

        // Act
        var actual = input.Chain(_ => { /* something interesting */ });

        // Assert
        actual.ShouldBeSameAs(input);
    }

    [Fact]
    public void Can_Convert_Object_To_Result_Success()
    {
        // Arrange
        MyPocoClass? sut = new();

        // Act
        var actual = sut.ToResult();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.Value.ShouldBeSameAs(sut);
    }

    [Fact]
    public void Can_Convert_Object_To_Result_NotFound_Without_ErrorMessage()
    {
        // Arrange
        MyPocoClass? sut = default;

        // Act
        var actual = sut.ToResult();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.Value.ShouldBeNull();
        actual.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public void Can_Convert_Object_To_Result_NotFound_With_ErrorMessage()
    {
        // Arrange
        MyPocoClass? sut = default;

        // Act
        var actual = sut.ToResult("My error message");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.Value.ShouldBeNull();
        actual.ErrorMessage.ShouldBe("My error message");
    }

    [Fact]
    public void Can_Transform_Object_Using_ExtensionMethod()
    {
        // Arrange
        var sut = true;

        // Act
        var actual = sut.Transform(x => x ? "true" : "false");

        // Assert
        actual.ShouldBe("true");
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
        actual.Items.Select(x => x.Value).ToArray().ShouldBeEquivalentTo(new[] { "ITEM1", "ITEM2" });
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
        actual.Items.Select(x => x.Value).ToArray().ShouldBeEquivalentTo(new[] { "ITEM1", "ITEM2" });
    }

    private sealed class MyPocoClass
    {
        [Required]
        public string? Value { get; set; }
    }

    private sealed class MyPocoClassWithList
    {
        public List<MyPocoClass> Items { get; set; } = [];
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
