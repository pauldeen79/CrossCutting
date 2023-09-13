namespace CrossCutting.Utilities.Parsers.Tests.FunctionParseResultArguments;

public class LiteralArgumentBaseTests
{
    [Fact]
    public void GetValueResult_Is_Not_Supported()
    {
        // Arrange
        var sut = new LiteralArgumentBase("Test");

        // Act & assert
        sut.Invoking(x => x.GetValueResult(null, Substitute.For<IFunctionParseResultEvaluator>(), Substitute.For<IExpressionParser>(), CultureInfo.InvariantCulture))
           .Should().Throw<NotSupportedException>();
    }
}
