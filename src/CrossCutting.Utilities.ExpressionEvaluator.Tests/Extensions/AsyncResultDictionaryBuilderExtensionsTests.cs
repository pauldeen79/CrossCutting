namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class AsyncResultDictionaryBuilderExtensionsTests : TestBase
{
    private FunctionCallContext CreateFunctionCallContext() => new FunctionCallContext(new FunctionCallBuilder()
        .WithName("Dummy")
        .WithMemberType(MemberType.Function)
        .AddArguments("1"), CreateContext("Dummy"));

    [Fact]
    public async Task Add_Untyped_NonGeneric_Returns_Correct_Result()
    {
        // Arrange
        var sut = new AsyncResultDictionaryBuilder();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add(context, 0, "MyName");

        // Assert
        var dict = await result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        dict.First().Value.Status.ShouldBe(ResultStatus.Ok);
        dict.First().Value.GetValue().ShouldBe(1);
    }

    [Fact]
    public async Task Add_Untyped_Generic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new AsyncResultDictionaryBuilder();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add<int>(context, 0, "MyName");

        // Assert
        var dict = await result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        dict.First().Value.Status.ShouldBe(ResultStatus.Ok);
        dict.First().Value.GetValue().ShouldBe(1);
    }

    [Fact]
    public async Task Add_Typed_Generic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new AsyncResultDictionaryBuilder<int>();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add(context, 0, "MyName");

        // Assert
        var dict = await result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        var value = dict.First().Value();
        value.Status.ShouldBe(ResultStatus.Ok);
        value.GetValue().ShouldBe(1);
    }
}
