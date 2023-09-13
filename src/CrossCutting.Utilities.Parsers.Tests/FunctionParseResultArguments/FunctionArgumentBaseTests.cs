namespace CrossCutting.Utilities.Parsers.Tests.FunctionParseResultArguments;

public class FunctionArgumentBaseTests
{
    [Fact]
    public void GetValueResult_Is_Not_Supported()
    {
        // Arrange
        var sut = new FunctionArgumentBase(new FunctionParseResultBuilder().WithFunctionName("Test").Build());

        // Act & assert
        sut.Invoking(x => x.GetValueResult(null, Substitute.For<IFunctionParseResultEvaluator>(), Substitute.For<IExpressionParser>(), CultureInfo.InvariantCulture))
           .Should().Throw<NotSupportedException>();
    }
}
