namespace CrossCutting.ProcessingPipeline.Tests.Decorators;

public class ExceptionDecoratorTests : TestBase
{
    public class ProcessAsync : ExceptionDecoratorTests
    {
        [Fact]
        public async Task Returns_Same_Result_On_Execution_Without_Exception()
        {
            // Arrange
            var pipelineComponent = Fixture.Create<IPipelineComponent<string>>();
            pipelineComponent.ProcessAsync(Arg.Any<PipelineContext<string>>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = new ExceptionDecorator<string>(pipelineComponent);
            var context = new PipelineContext<string>("hello world");

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Error_On_Execution_With_Exception()
        {
            // Arrange
            var pipelineComponent = Fixture.Create<IPipelineComponent<string>>();
            pipelineComponent.ProcessAsync(Arg.Any<PipelineContext<string>>(), Arg.Any<CancellationToken>()).Throws(new InvalidOperationException("Kaboom"));
            var sut = new ExceptionDecorator<string>(pipelineComponent);
            var context = new PipelineContext<string>("hello world");

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }
    }
}
