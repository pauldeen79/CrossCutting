namespace CrossCutting.Common.Tests.Results;

public class LazyResultDictionaryTests
{
    [Fact]
    public void Can_Get_Result_Untyped()
    {
        // Arrange
        var sut = new LazyResultDictionary
        {
            { "Item1", () => Result.Success("Long running operation") }
        };

        // Act
        var result = sut["Item1"];

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.GetValue().ShouldBe("Long running operation");
    }

    [Fact]
    public void Can_Get_Result_Typed()
    {
        // Arrange
        var sut = new LazyResultDictionary<string>
        {
            { "Item1", () => Result.Success("Long running operation") }
        };

        // Act
        var result = sut["Item1"];

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Long running operation");
    }
}
