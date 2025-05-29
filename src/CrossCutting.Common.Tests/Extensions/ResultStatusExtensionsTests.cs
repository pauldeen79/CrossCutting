namespace CrossCutting.Common.Tests.Extensions;

public class ResultStatusExtensionsTests
{
    [Fact]
    public void ToResult_Untyped_Returns_New_Result_Instance()
    {
        // Arrange
        var status = ResultStatus.BadGateway;

        // Act
        var result = status.ToResult("there is a very bad gateway here", exception: new InvalidOperationException("error"));

        // Assert
        result.Status.ShouldBe(status);
        result.ErrorMessage.ShouldBe("there is a very bad gateway here");
        result.Exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public void ToResult_Typed_Returns_New_Result_Instance()
    {
        // Arrange
        var status = ResultStatus.BadGateway;

        // Act
        var result = status.ToTypedResult("Value", "there is a very bad gateway here", exception: new InvalidOperationException("error"));

        // Assert
        result.Status.ShouldBe(status);
        result.Value.ShouldBe("Value");
        result.ErrorMessage.ShouldBe("there is a very bad gateway here");
        result.Exception.ShouldBeOfType<InvalidOperationException>();
    }
}
