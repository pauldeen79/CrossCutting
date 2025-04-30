namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    [Fact]
    public void Can_Evaluate_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-1 + 1 + 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1);
    }

    [Fact]
    public void Can_Evaluate_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(state)";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, state: "hello world", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    [Fact]
    public void Can_Evaluate_Property_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "state.Length";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = sut.Evaluate(CreateContext(expression, state: "hello world", evaluator: sut, settings: settings));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Length);
    }

    [Fact]
    public void Can_Evaluate_Method_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "state.ToString()";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = sut.Evaluate(CreateContext(expression, state: "hello world", evaluator: sut, settings: settings));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".ToString());
    }

    [Fact]
    public void Can_Evaluate_Generic_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyGenericFunction<System.String>()";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Can_Evaluate_Primitive_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "null";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(null);
    }

    [Fact]
    public void Can_Evaluate_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "13";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public void Can_Evaluate_Negative_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-13";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(-13);
    }

    [Fact]
    public void Can_Evaluate_Negate_Boolean_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "!false";

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "\"my string value\"";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("my string value");
    }

    [Fact]
    public void Can_Evaluate_Formattable_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "$\"my value with {state} items\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression, state: "replaced", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ToStringWithDefault().ShouldBe("my value with replaced items");
    }

    [Theory]
    [InlineData("(1 + 1 + 1) == 3 && \"true\" == \"true\"")]
    [InlineData("(1 == 1) && 2 == 2")]
    [InlineData("MyFunction(\"a\") != MyFunction(\"b\")")]
    [InlineData("MyFunction(\"+\") != MyFunction(\"-\")")]
    public void Can_Evaluate_Operator_Expression(string expression)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Operator_Expression_With_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("!true", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Can_Evaluate_Operator_Expression_With_Double_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("!!true", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToString_DotExpression_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("true.ToString()", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true.ToString());
    }

    [Fact]
    public void Evaluate_ToString_DotExpression_On_Null_Expression_Gives_Correct_Result()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("null.ToString()", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("null is null, cannot evaluate method ToString");
    }

    [Fact]
    public void Can_Evaluate_Expression_With_IsNull_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("IsNull(\"some value that is not null\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToCamelCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToCamelCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToCamelCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToLowerCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToLowerCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToLower(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToPascalCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToPascalCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToPascalCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_ToUpperCase_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("ToUpperCase(\"expression\")", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToUpper(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_Date_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("Date(2025, 1, 1)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_DateTime_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("DateTime(2025, 1, 1, 12, 13, 14)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 12, 13, 14, DateTimeKind.Unspecified));
    }

    [Fact]
    public void Can_Evaluate_Expression_With_Indexer()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state[1]", evaluator: sut, state: new object[] { 1, 2, 3 }));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(2);
    }

    [Fact]
    public void Can_Evaluate_Expression_With_Indexer_And_Method()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = sut.Evaluate(CreateContext("state[1].ToString()", evaluator: sut, state: new object[] { 1, 2, 3 }, settings: settings));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("2");
    }

    [Fact]
    public void Can_Evaluate_DateTime_Year_Date()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Date", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Date);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Year()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Year", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Year);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Month()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Month", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Month);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Day()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Day", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Day);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Hour()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Hour", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Hour);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Minute()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Minute", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Minute);
    }

    [Fact]
    public void Can_Evaluate_DateTime_Second()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Second", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Second);
    }

    [Fact]
    public void Can_Evaluate_String_Length()
    {
        // Arrange
        var stringValue = "Hello world!";
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Length", evaluator: sut, state: stringValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(stringValue.Length);
    }

    [Fact]
    public void Can_Evaluate_Array_Length()
    {
        // Arrange
        var arrayValue = new string[] { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Length", evaluator: sut, state: arrayValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(arrayValue.Length);
    }

    [Fact]
    public void Can_Evaluate_Enumerable_Count()
    {
        // Arrange
        var enumerableValue = new string[] { "Hello", "", "world!" }.AsEnumerable();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("Count(state)", evaluator: sut, state: enumerableValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(enumerableValue.Count());
    }

    [Fact]
    public void Can_Evaluate_Collection_Count()
    {
        // Arrange
        var collectionValue = new List<string> { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("state.Count", evaluator: sut, state: collectionValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(collectionValue.Count);
    }

    [Fact]
    public void Can_Evaluate_Coalesce_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("Coalesce(null, null, 13)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public void Can_Evaluate_String_Substring_DotExpression_One_Argument()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("\"hello world\".Substring(6)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Substring(6));
    }

    [Fact]
    public void Can_Evaluate_String_Substring_DotExpression_Two_Arguments()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(CreateContext("\"hello world\".Substring(0, 5)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Substring(0, 5));
    }

    [Fact]
    public void Can_Get_Some_Stuff_From_InterpolatedString_Like_How_We_Want_To_In_ClassFramework()
    {
        // Observations:
        // 1. We need to 'new' an InterpolatedStringExpressionComponent
        // 2. We need to wrap the string within $""
        // Otherwise, it works perfectly :)

        // Arrange
        var sut = new InterpolatedStringExpressionComponent();
        var state = new DeferredResultDictionaryBuilder<object?>()
            .Add("class.Name", () => Result.Success<object?>("MyClass"))
            .Build();
        var context = new ExpressionEvaluatorContext("$\"public class {class.Name}\"", new ExpressionEvaluatorSettingsBuilder(), CreateSut(), state);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.ToString().ShouldBe("public class MyClass");
    }

    [Fact]
    public void Can_Parse_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "(-1 + 1) + 1";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Wrong_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of member MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Argument Input is not of type System.String");
    }

    [Fact]
    public void Can_Parse_DateTime_Year_Date()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Date", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Date.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Year()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Year", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Year.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Month()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Month", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Month.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Day()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Day", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Day.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Hour()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Hour", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Hour.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Minute()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Minute", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Minute.GetType());
    }

    [Fact]
    public void Can_Parse_DateTime_Second()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Second", evaluator: sut, state: dateTime));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Second.GetType());
    }

    [Fact]
    public void Can_Parse_String_Length()
    {
        // Arrange
        var stringValue = "Hello world!";
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Length", evaluator: sut, state: stringValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(stringValue.Length.GetType());
    }

    [Fact]
    public void Can_Parse_Array_Length()
    {
        // Arrange
        var arrayValue = new string[] { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Length", evaluator: sut, state: arrayValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(arrayValue.Length.GetType());
    }

    [Fact]
    public void Can_Parse_Enumerable_Count()
    {
        // Arrange
        var enumerableValue = new string[] { "Hello", "", "world!" }.AsEnumerable();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("Count(state)", evaluator: sut, state: enumerableValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(enumerableValue.Count().GetType());
    }

    [Fact]
    public void Can_Parse_Collection_Count()
    {
        // Arrange
        var collectionValue = new List<string> { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("state.Count", evaluator: sut, state: collectionValue));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(collectionValue.Count.GetType());
    }

    [Fact]
    public void Can_Parse_Coalesce_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("Coalesce(null, null, 13)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBeNull(); // made this design decision because of the dynamic nature of this function. we can't evaluate the values while parsing
    }

    [Fact]
    public void Can_Parse_String_Substring_DotExpression_One_Argument()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("\"hello world\".Substring(6)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe("hello world".Substring(6).GetType());
    }

    [Fact]
    public void Can_Parse_String_Substring_DotExpression_Two_Arguments()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Parse(CreateContext("\"hello world\".Substring(0, 5)", evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe("hello world".Substring(0, 5).GetType());
    }

    [Fact]
    public void Can_Parse_Method_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "1.ToString()";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut, settings: settings));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(typeof(string));
    }

    [Fact]
    public void Can_Skip_Validation_On_ArgumentTypes_Using_Setting()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = sut.Parse(CreateContext(expression, settings: new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(false), evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Unknown_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(state)";
        var context = new DeferredResultDictionaryBuilder<object?>()
            .Add("state", Result.NoContent<object?>)
            .Build();

        // Act
        var result = sut.Parse(CreateContext(expression, context: context, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Parse_Function_With_Parse_Error()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(error)";

        // Act
        var result = sut.Parse(CreateContext(expression, evaluator: sut));

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of member MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Unknown expression type found in fragment: error");
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddSingleton<IMember, MyFunction>()
            .AddSingleton<IMember, MyGenericFunction>()
            .BuildServiceProvider();
    }

    private ServiceProvider Provider { get; set; }

    private IExpressionEvaluator CreateSut() => Provider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose() => Provider.Dispose();

    [FunctionName("MyFunction")]
    [FunctionArgument("Input", typeof(string))]
    private sealed class MyFunction : IFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
            => EvaluateTyped(context).TryCastAllowNull<object?>();

        public Result<string> EvaluateTyped(FunctionCallContext context)
            => new ResultDictionaryBuilder()
                .Add("Input", () => context.GetArgumentValueResult<string>(0, "Input"))
                .Build()
                .OnSuccess(results => Result.Success(results.GetValue<string>("Input").ToUpperInvariant()));
    }

    [FunctionName("MyGenericFunction")]
    [FunctionTypeArgument("T", "Type argument to use")]
    private sealed class MyGenericFunction : IGenericFunction
    {
        public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
            => Result.Success<object?>(typeof(T));
    }
}
