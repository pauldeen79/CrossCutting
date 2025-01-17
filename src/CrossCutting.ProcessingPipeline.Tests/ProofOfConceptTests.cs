namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    protected static Pipeline<object?> CreateResponselessSut(Func<CallInfo, Result> processDelegate)
    {
        var pipelineComponent = Substitute.For<IPipelineComponent<object?>>();

        pipelineComponent
            .ProcessAsync(Arg.Any<PipelineContext<object?>>(), Arg.Any<CancellationToken>())
            .Returns(processDelegate);

        return new Pipeline<object?>([pipelineComponent]);
    }

    protected static Pipeline<object?, StringBuilder> CreateResponsefulSut(Func<CallInfo, Result> processDelegate)
    {
        var pipelineComponent = Substitute.For<IPipelineComponent<object?, StringBuilder>>();

        pipelineComponent
            .ProcessAsync(Arg.Any<PipelineContext<object?, StringBuilder>>(), Arg.Any<CancellationToken>())
            .Returns(processDelegate);

        return new Pipeline<object?, StringBuilder>([pipelineComponent]);
    }

    public class Pipeline_With_Response : ProofOfConceptTests
    {
        [Fact]
        public async Task Can_ProcessAsync_Pipeline_With_Component()
        {
            // Arrange
            PipelineContext<object?, StringBuilder>? context = null;
            var sut = CreateResponsefulSut(x =>
            {
                context = x.ArgAt<PipelineContext<object?, StringBuilder>>(0);
                x.ArgAt<PipelineContext<object?, StringBuilder>>(0).Response.Append("2");
                return Result.Continue<object?>();
            });

            // Act
            var result = await sut.ProcessAsync(request: 1, seed: new StringBuilder());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Request.Should().BeEquivalentTo(1);
            result.GetValueOrThrow().ToString().Should().Be("2");
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status()
        {
            // Arrange
            var sut = CreateResponsefulSut(x => Result.Error<object?>("Kaboom"));

            // Act
            var result = await sut.ProcessAsync(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status_And_CancellationToken()
        {
            // Arrange
            var sut = CreateResponsefulSut(x => Result.Error<object?>("Kaboom"));

            // Act
            var result = await sut.ProcessAsync(request: 1, cancellationToken: new CancellationToken());

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Components_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }

    public class Pipeline_Without_Response : ProofOfConceptTests
    {
        [Fact]
        public async Task Can_ProcessAsync_Pipeline_With_Component()
        {
            // Arrange
            PipelineContext<object?>? context = null;
            var sut = CreateResponselessSut(x =>
            {
                context = x.ArgAt<PipelineContext<object?>>(0);
                return Result.Continue<object?>();
            });

            // Act
            var result = await sut.ProcessAsync(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Request.Should().BeEquivalentTo(1);
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status()
        {
            // Arrange
            var sut = CreateResponselessSut(x => Result.Error<object?>("Kaboom"));

            // Act
            var result = await sut.ProcessAsync(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Component_Using_Non_Success_Status_And_CancellationToken()
        {
            // Arrange
            var sut = CreateResponselessSut(x => Result.Error<object?>("Kaboom"));

            // Act
            var result = await sut.ProcessAsync(request: 1, new CancellationToken());

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("An error occured while processing the pipeline. See the inner results for more details.");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Components_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }

    public class PipelineComponent_Without_Context()
    {
        [Fact]
        public async Task Can_Call_ProcessAsync_Without_CancellationToken()
        {
            // Arrange
            var sut = Substitute.For<IPipelineComponent<object?>>();
            sut
                .ProcessAsync(Arg.Any<PipelineContext<object?>>(), Arg.Any<CancellationToken>())
                .Returns(Result.NotImplemented());
            var context = new PipelineContext<object?>(1);

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.Should().Be(ResultStatus.NotImplemented);
        }
    }

    public class PipelineComponent_With_Context()
    {
        [Fact]
        public async Task Can_Call_ProcessAsync_Without_CancellationToken()
        {
            // Arrange
            var sut = Substitute.For<IPipelineComponent<object?, StringBuilder>>();
            sut
                .ProcessAsync(Arg.Any<PipelineContext<object?, StringBuilder>>(), Arg.Any<CancellationToken>())
                .Returns(Result.NotImplemented());
            var context = new PipelineContext<object?, StringBuilder>(1, new StringBuilder());

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.Should().Be(ResultStatus.NotImplemented);
        }
    }
}
