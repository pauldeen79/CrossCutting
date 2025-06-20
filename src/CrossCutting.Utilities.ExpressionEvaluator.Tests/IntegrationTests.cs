﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    [Fact]
    public async Task Can_Evaluate_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task Can_Evaluate_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task Can_Evaluate_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-1 + 1 + 1";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1);
    }

    [Fact]
    public async Task Can_Evaluate_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(state)";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, state: "hello world", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    [Fact]
    public async Task Can_Evaluate_Typed_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyTypedFunction(state)";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, state: "hello world", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    [Fact]
    public async Task Can_Evaluate_Property_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "state.Length";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, state: "hello world", evaluator: sut, settings: settings), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Length);
    }

    [Fact]
    public async Task Can_Evaluate_Instance_Method_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "state.ToString()";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, state: "hello world", evaluator: sut, settings: settings), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".ToString());
    }

    [Fact]
    public async Task Can_Evaluate_Generic_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyGenericFunction<System.String>()";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public async Task Can_Evaluate_Primitive_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "null";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(null);
    }

    [Fact]
    public async Task Can_Evaluate_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "13";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public async Task Can_Evaluate_Negative_Numeric_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "-13";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(-13);
    }

    [Fact]
    public async Task Can_Evaluate_Negate_Boolean_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "!false";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task Can_Evaluate_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "\"my string value\"";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("my string value");
    }

    [Fact]
    public async Task Can_Evaluate_Formattable_String_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "$\"my value with {state} items\"";

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, state: "replaced", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ToStringWithDefault().ShouldBe("my value with replaced items");
    }

    [Theory]
    [InlineData("(1 + 1 + 1) == 3 && \"true\" == \"true\"")]
    [InlineData("(1 == 1) && 2 == 2")]
    [InlineData("MyFunction(\"a\") != MyFunction(\"b\")")]
    [InlineData("MyFunction(\"+\") != MyFunction(\"-\")")]
    public async Task Can_Evaluate_Operator_Expression(string expression)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task Can_Evaluate_Operator_Expression_With_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("!true", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public async Task Can_Evaluate_Operator_Expression_With_Double_Bang_Operator()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("!!true", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_ToString_Instance_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("true.ToString()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true.ToString());
    }

    [Fact]
    public async Task Evaluate_ToString_Instance_Method_On_Null_Expression_Gives_Correct_Result()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("null.ToString()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("null is null, cannot evaluate method ToString");
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_IsNull_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("IsNull(\"some value that is not null\")", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_ToCamelCase_Instance_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"expression\".ToCamelCase()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToCamelCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_ToLower_Instance_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"expression\".ToLower()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToLower(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_ToPascalCase_Instance_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"expression\".ToPascalCase()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToPascalCase(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_ToUpper_Instance_Method()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"expression\".ToUpper()", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("expression".ToUpper(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_Date_Instance_Constructor()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("new DateTime(2025, 1, 1)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_DateTime_Instance_Constructor()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("new DateTime(2025, 1, 1, 12, 13, 14)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(new DateTime(2025, 1, 1, 12, 13, 14, DateTimeKind.Unspecified));
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_Indexer()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateTypedAsync<int>(CreateContext("state[1]", evaluator: sut, state: new object[] { 1, 2, 3 }), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(2);
    }

    [Fact]
    public async Task Can_Evaluate_Expression_With_Indexer_And_Method()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state[1].ToString()", evaluator: sut, state: new object[] { 1, 2, 3 }, settings: settings), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("2");
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Year_Date()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Date", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Date);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Year()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Year", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Year);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Month()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Month", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Month);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Day()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Day", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Day);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Hour()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Hour", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Hour);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Minute()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Minute", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Minute);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_Second()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Second", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.Second);
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddYears()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddYears(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddYears(1));
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddMonths()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddMonths(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddMonths(1));
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddDays()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddDays(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddDays(1));
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddHours()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddHours(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddHours(1));
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddMinutes()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddMinutes(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddMinutes(1));
    }

    [Fact]
    public async Task Can_Evaluate_DateTime_AddSeconds()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.AddSeconds(1)", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dateTime.AddSeconds(1));
    }

    [Fact]
    public async Task Can_Evaluate_String_Length()
    {
        // Arrange
        var stringValue = "Hello world!";
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Length", evaluator: sut, state: stringValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(stringValue.Length);
    }

    [Fact]
    public async Task Can_Evaluate_Array_Length()
    {
        // Arrange
        var arrayValue = new string[] { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Length", evaluator: sut, state: arrayValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(arrayValue.Length);
    }

    [Fact]
    public async Task Can_Evaluate_Enumerable_Count()
    {
        // Arrange
        var enumerableValue = new string[] { "Hello", "", "world!" }.AsEnumerable();
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("Count(state)", evaluator: sut, state: enumerableValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(enumerableValue.Count());
    }

    [Fact]
    public async Task Can_Evaluate_Collection_Count()
    {
        // Arrange
        var collectionValue = new List<string> { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("state.Count", evaluator: sut, state: collectionValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(collectionValue.Count);
    }

    [Fact]
    public async Task Can_Evaluate_Coalesce_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("Coalesce(null, null, 13)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public async Task Can_Evaluate_String_Substring_Instance_Method_One_Argument()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"hello world\".Substring(6)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Substring(6));
    }

    [Fact]
    public async Task Can_Evaluate_String_Substring_Instance_Method_Two_Arguments()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.EvaluateAsync(CreateContext("\"hello world\".Substring(0, 5)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello world".Substring(0, 5));
    }

    [Fact]
    public async Task Can_Get_Some_Stuff_From_InterpolatedString_Like_How_We_Want_To_In_ClassFramework_Using_ExpressionComponent()
    {
        // Observations:
        // 1. We need to 'new' an InterpolatedStringExpressionComponent (optional - see alternative test below)
        // 2. We need to wrap the string within $""
        // Otherwise, it works perfectly :)

        // Arrange
        var sut = new InterpolatedStringExpressionComponent();
        var state = new AsyncResultDictionaryBuilder<object?>()
            .Add("class.Name", Result.Success<object?>("MyClass"))
            .BuildDeferred();
        var context = new ExpressionEvaluatorContext("$\"public class {class.Name}\"", new ExpressionEvaluatorSettingsBuilder(), CreateSut(), state);

        // Act
        var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.ToString().ShouldBe("public class MyClass");
    }

    [Fact]
    public async Task Can_Get_Some_Stuff_From_InterpolatedString_Like_How_We_Want_To_In_ClassFramework_Using_ExpressionEvaluator()
    {
        // Arrange
        var sut = CreateSut();
        var state = new AsyncResultDictionaryBuilder<object?>()
            .Add("class.Name", Result.Success<object?>("MyClass"))
            .BuildDeferred();
        var context = new ExpressionEvaluatorContext("$\"public class {class.Name}\"", new ExpressionEvaluatorSettingsBuilder(), CreateSut(), state);

        // Act
        var result = await sut.EvaluateTypedAsync<GenericFormattableString>(context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.ToString().ShouldBe("public class MyClass");
    }

    [Fact]
    public async Task Can_Perform_Logic_In_Property_Like_We_Do_In_ClassFramework_Untyped()
    {
        // Arrange
        var state = new AsyncResultDictionaryBuilder();
        var functionParser = Substitute.For<IFunctionParser>();
        functionParser
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));

        var context = new FunctionCallContext(new DotExpressionComponentState(CreateContext("Dummy()", state), functionParser, Result.Success<object?>("Dummy"), "Dummy", typeof(string)) { Value = "hello" });

        // Act
        var result = (await new AsyncResultDictionaryBuilder()
            .Add("instance", context.GetInstanceValueResult<string>())
            .Build())
            .OnSuccess(results => results.GetValue<string>("instance"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello");
    }

    [Fact]
    public async Task Can_Perform_Logic_In_Property_Like_We_Do_In_ClassFramework_Untyped_On_Error()
    {
        // Arrange
        var state = new AsyncResultDictionaryBuilder();
        var functionParser = Substitute.For<IFunctionParser>();
        functionParser
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));

        var context = new FunctionCallContext(new DotExpressionComponentState(CreateContext("Dummy()", state), functionParser, Result.Success<object?>("Dummy"), "Dummy", typeof(string)) { Value = "hello" });

        // Act
        var result = (await new AsyncResultDictionaryBuilder()
            .Add("instance", context.GetInstanceValueResult<string>())
            .Add("error", Result.Error("Kaboom"))
            .Build())
            .OnSuccess(results => results.GetValue<string>("instance"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public async Task Can_Perform_Logic_In_Property_Like_We_Do_In_ClassFramework_Typed()
    {
        // Arrange
        var state = new AsyncResultDictionaryBuilder();
        var functionParser = Substitute.For<IFunctionParser>();
        functionParser
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));

        var context = new FunctionCallContext(new DotExpressionComponentState(CreateContext("Dummy()", state), functionParser, Result.Success<object?>("Dummy"), "Dummy", typeof(string)) { Value = "hello" });

        // Act
        var result = (await new AsyncResultDictionaryBuilder<object?>()
            .Add("instance", context.GetInstanceValueResult<string>())
            .Build())
            .OnSuccess(results => results.GetValue("instance"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("hello");
    }

    [Fact]
    public async Task Can_Perform_Logic_In_Property_Like_We_Do_In_ClassFramework_Typed_On_Error()
    {
        // Arrange
        var state = new AsyncResultDictionaryBuilder();
        var functionParser = Substitute.For<IFunctionParser>();
        functionParser
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));

        var context = new FunctionCallContext(new DotExpressionComponentState(CreateContext("Dummy()", state), functionParser, Result.Success<object?>("Dummy"), "Dummy", typeof(string)) { Value = "hello" });

        // Act
        var result = (await new AsyncResultDictionaryBuilder<object?>()
            .Add("instance", context.GetInstanceValueResult<string>())
            .Add("error", Result.Error<object?>("Kaboom"))
            .Build())
            .OnSuccess(results => results.GetValue("instance"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public async Task Can_Parse_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public async Task Can_Parse_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public async Task Can_Parse_Mathematic_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "(-1 + 1) + 1";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public async Task Can_Parse_Function_With_Wrong_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of member MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Argument Input is not of type System.String");
    }

    [Fact]
    public async Task Can_Parse_DateTime_Year_Date()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Date", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Date.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Year()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Year", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Year.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Month()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Month", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Month.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Day()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Day", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Day.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Hour()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Hour", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Hour.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Minute()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Minute", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Minute.GetType());
    }

    [Fact]
    public async Task Can_Parse_DateTime_Second()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 1, 11, 10, 9, DateTimeKind.Unspecified);
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Second", evaluator: sut, state: dateTime), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(dateTime.Second.GetType());
    }

    [Fact]
    public async Task Can_Parse_String_Length()
    {
        // Arrange
        var stringValue = "Hello world!";
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Length", evaluator: sut, state: stringValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(stringValue.Length.GetType());
    }

    [Fact]
    public async Task Can_Parse_Array_Length()
    {
        // Arrange
        var arrayValue = new string[] { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Length", evaluator: sut, state: arrayValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(arrayValue.Length.GetType());
    }

    [Fact]
    public async Task Can_Parse_Enumerable_Count()
    {
        // Arrange
        var enumerableValue = new string[] { "Hello", "", "world!" }.AsEnumerable();
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("Count(state)", evaluator: sut, state: enumerableValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(enumerableValue.Count().GetType());
    }

    [Fact]
    public async Task Can_Parse_Collection_Count()
    {
        // Arrange
        var collectionValue = new List<string> { "Hello", "", "world!" };
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("state.Count", evaluator: sut, state: collectionValue), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(collectionValue.Count.GetType());
    }

    [Fact]
    public async Task Can_Parse_Coalesce_Function()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("Coalesce(null, null, 13)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBeNull(); // made this design decision because of the dynamic nature of this function. we can't evaluate the values while parsing
    }

    [Fact]
    public async Task Can_Parse_String_Substring_Instance_Method_One_Argument()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("\"hello world\".Substring(6)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe("hello world".Substring(6).GetType());
    }

    [Fact]
    public async Task Can_Parse_String_Substring_Instance_Method_Two_Arguments()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.ParseAsync(CreateContext("\"hello world\".Substring(0, 5)", evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe("hello world".Substring(0, 5).GetType());
    }

    [Fact]
    public async Task Can_Parse_Instance_Method_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "1.ToString()";
        var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut, settings: settings), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ResultType.ShouldBe(typeof(string));
    }

    [Fact]
    public async Task Can_Skip_Validation_On_ArgumentTypes_Using_Setting()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(123)";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, settings: new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(false), evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public async Task Can_Parse_Function_With_Unknown_ArgumentType()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(state)";
        var state = new AsyncResultDictionaryBuilder<object?>()
            .Add("state", Result.NoContent<object?>)
            .BuildDeferred();

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, state: state, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public async Task Can_Parse_Function_With_Parse_Error()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(error)";

        // Act
        var result = await sut.ParseAsync(CreateContext(expression, evaluator: sut), CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Validation of member MyFunction failed, see validation errors for more details");
        result.ValidationErrors.Count.ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Unknown expression type found in fragment: error");
    }

    [Fact]
    public void Can_Get_MemberDescriptors()
    {
        // Arrange
        var sut = Provider.GetRequiredService<IMemberDescriptorProvider>();

        // Act
        var descriptors = sut.GetAll();

        // Assert
        descriptors.Status.ShouldBe(ResultStatus.Ok);
        descriptors.Value.ShouldNotBeEmpty();
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddSingleton<IMember, MyFunction>()
            .AddSingleton<IMember, MyTypedFunction>()
            .AddSingleton<IMember, MyGenericFunction>()
            .BuildServiceProvider();
    }

    private ServiceProvider Provider { get; set; }

    private IExpressionEvaluator CreateSut() => Provider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose() => Provider.Dispose();

    [MemberName("MyFunction")]
    [MemberArgument("Input", typeof(string))]
    private sealed class MyFunction : IFunction
    {
        public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
            => (await new AsyncResultDictionaryBuilder()
                .Add("Input", context.GetArgumentValueResultAsync<string>(0, "Input", token))
                .Build().ConfigureAwait(false))
                .OnSuccess(results => Result.Success<object?>(results.GetValue<string>("Input").ToUpperInvariant()));
    }

    [MemberName("MyTypedFunction")]
    [MemberArgument("Input", typeof(string))]
    private sealed class MyTypedFunction : IFunction<string>
    {
        public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
            => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

        public async Task<Result<string>> EvaluateTypedAsync(FunctionCallContext context, CancellationToken token)
            => (await new AsyncResultDictionaryBuilder()
                .Add("Input", context.GetArgumentValueResultAsync<string>(0, "Input", token))
                .Build().ConfigureAwait(false))
                .OnSuccess(results => Result.Success(results.GetValue<string>("Input").ToUpperInvariant()));
    }

    [MemberName("MyGenericFunction")]
    [MemberTypeArgument("T", "Type argument to use")]
    private sealed class MyGenericFunction : IGenericFunction
    {
        public Task<Result<object?>> EvaluateGenericAsync<T>(FunctionCallContext context, CancellationToken token)
            => Task.FromResult(Result.Success<object?>(typeof(T)));
    }
}
