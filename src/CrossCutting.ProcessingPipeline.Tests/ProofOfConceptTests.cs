namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    protected static Pipeline<object?> CreateResponselessSut(Func<CallInfo, Result> processDelegate)
    {
        var pipelineComponent = Substitute.For<IPipelineComponent<object?>>();

        pipelineComponent
            .ExecuteAsync(Arg.Any<object?>(), Arg.Any<ICommandService>(), Arg.Any<CancellationToken>())
            .Returns(processDelegate);

        return new Pipeline<object?>(new PassThroughDecorator(), [pipelineComponent]);
    }

    public class Pipeline_Without_Response : ProofOfConceptTests
    {
        [Fact]
        public async Task Can_Process_Pipeline_With_Component()
        {
            // Arrange
            object? command = null;
            var sut = CreateResponselessSut(x =>
            {
                command = x.ArgAt<object?>(0);
                return Result.Continue<object?>();
            });
            var commandService = Substitute.For<ICommandService>();

            // Act
            var result = await sut.ExecuteAsync(command: 1, commandService);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            command.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status()
        {
            // Arrange
            var sut = CreateResponselessSut(x => Result.Error<object?>("Kaboom"));
            var commandService = Substitute.For<ICommandService>();

            // Act
            var result = await sut.ExecuteAsync(command: 1, commandService);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status_And_CancellationToken()
        {
            // Arrange
            var sut = CreateResponselessSut(x => Result.Error<object?>("Kaboom"));
            var commandService = Substitute.For<ICommandService>();

            // Act
            var result = await sut.ExecuteAsync(command: 1, commandService, new CancellationToken());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Decorator_Throws_ArgumentNullException()
        {
            // Act & Assert
            Action a = () => _ = new Pipeline<object?>(decorator: null!, components: []);
            a.ShouldThrow<ArgumentNullException>()
             .ParamName.ShouldBe("decorator");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Components_Throws_ArgumentNullException()
        {
            // Act & Assert
            Action a = () => _ = new Pipeline<object?>(new PassThroughDecorator(), components: null!);
            a.ShouldThrow<ArgumentNullException>()
             .ParamName.ShouldBe("components");
        }
    }

    public class PipelineComponent()
    {
        [Fact]
        public async Task Can_Call_Process_Without_CancellationToken()
        {
            // Arrange
            var sut = Substitute.For<IPipelineComponent<object?>>();
            sut
                .ExecuteAsync(Arg.Any<object?>(), Arg.Any<ICommandService>(), Arg.Any<CancellationToken>())
                .Returns(Result.NotImplemented());
            var command = 1;
            var commandService = Substitute.For<ICommandService>();

            // Act
            var result = await sut.ExecuteAsync(command, commandService, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotImplemented);
        }
    }
}
