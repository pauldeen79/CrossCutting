namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class ResultDictionaryBuilderExtensionsTests : TestBase
{
    private FunctionCallContext CreateFunctionCallContext() => new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").AddArguments("1"), CreateContext("Dummy"), MemberType.Function);

    [Fact]
    public void Add_Untyped_NonGeneric_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ResultDictionaryBuilder();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add(context, 0, "MyName");

        // Assert
        var dict = result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        dict.First().Value.Status.ShouldBe(ResultStatus.Ok);
        dict.First().Value.GetValue().ShouldBe(1);
    }

    [Fact]
    public void Add_Untyped_Generic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ResultDictionaryBuilder();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add<int>(context, 0, "MyName");

        // Assert
        var dict = result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        dict.First().Value.Status.ShouldBe(ResultStatus.Ok);
        dict.First().Value.GetValue().ShouldBe(1);
    }

    [Fact]
    public void Add_Typed_Generic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ResultDictionaryBuilder<int>();
        var context = CreateFunctionCallContext();

        // Act
        var result = sut.Add(context, 0, "MyName");

        // Assert
        var dict = result.Build();
        dict.Count.ShouldBe(1);
        dict.First().Key.ShouldBe("MyName");
        dict.First().Value.Status.ShouldBe(ResultStatus.Ok);
        dict.First().Value.GetValue().ShouldBe(1);
    }
}
