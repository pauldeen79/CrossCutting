namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.DotExpressionComponents;

public class ReflectionMethodDotExpressionComponentTests : TestBase<ReflectionMethodDotExpressionComponent>
{
    protected IFunctionParser FunctionParser => ClassFactories.GetOrCreate<IFunctionParser>(ClassFactory);

    public ReflectionMethodDotExpressionComponentTests() : base()
    {
        // replace the real functionparser, so we can control its behavior
        ClassFactories[typeof(IFunctionParser)] = Substitute.For<IFunctionParser>();
    }

    public class EvaluateAsync : ReflectionMethodDotExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Unsuccessful_Result_From_FunctionParseResult()
        {
            // Arrange
            FunctionParser
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Error<FunctionCall>("Kaboom"));
            var sut = CreateSut();
            var context = CreateContext("Dummy", settings: new ExpressionEvaluatorSettingsBuilder().WithAllowReflection());

            // Act
            var result = await sut.EvaluateAsync(new DotExpressionComponentState(context, FunctionParser, Result.NoContent<object?>("Kaboom"), "Dummy"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
