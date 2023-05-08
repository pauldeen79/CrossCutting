namespace CrossCutting.Utilities.Parsers.Tests.FunctionParseResultArguments;

public class LiteralArgumentBaseTests
{
    [Fact]
    public void GetValueResult_Is_Not_Supported()
    {
        // Arrange
        var sut = new LiteralArgumentBase("Test");

        // Act & assert
        sut.Invoking(x => x.GetValueResult(null, new Mock<IFunctionParseResultEvaluator>().Object, new Mock<IExpressionParser>().Object, CultureInfo.InvariantCulture))
           .Should().Throw<NotSupportedException>();
    }
}
