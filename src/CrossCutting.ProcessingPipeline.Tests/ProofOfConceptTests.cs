namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    [Fact]
    public void Can_Create_Pipeline()
    {
        // Arrange
        var builder = new PipelineBuilder<object?, object?>();

        // Act
        var pipeline = builder
            .AddFeature(new MyFeature())
            .Build();

        // Assert
        pipeline.Features.Should().ContainSingle();
        pipeline.Features.Single().Should().BeOfType<MyFeature>();
    }

    [Fact]
    public void Can_Add_Multiple_Features_Using_Array()
    {
        // Arrange
        var builder = new PipelineBuilder<object?, object?>();

        // Act
        var pipeline = builder
            .AddFeatures(new MyFeature(), new MyFeature())
            .Build();

        // Assert
        pipeline.Features.Should().HaveCount(2);
        pipeline.Features.Should().AllBeOfType<MyFeature>();
    }

    [Fact]
    public void Can_Add_Multiple_Features_Using_Enumerable()
    {
        // Arrange
        var builder = new PipelineBuilder<object?, object?>();

        // Act
        var pipeline = builder
            .AddFeatures(new[] { new MyFeature(), new MyFeature() }.AsEnumerable())
            .Build();

        // Assert
        pipeline.Features.Should().HaveCount(2);
        pipeline.Features.Should().AllBeOfType<MyFeature>();
    }

    [Fact]
    public void Can_Replace_Feature_On_Pipeline()
    {
        // Arrange
        var builder = new PipelineBuilder<object?, object?>()
            .AddFeature(new MyFeature());

        // Act
        var pipeline = builder
            .ReplaceFeature<MyFeature>(new MyReplacedFeature())
            .Build();

        // Assert
        pipeline.Features.Should().ContainSingle();
        pipeline.Features.Single().Should().BeOfType<MyReplacedFeature>();
    }

    [Fact]
    public void Can_Validate_PipelineBuilder()
    {
        // Arrange
        var builder = new PipelineBuilder<object?, object?> { Features = null! };

        // Act
        var validationResults = builder.Validate(new(builder));

        // Assert
        validationResults.Should().ContainSingle();
    }

    [Fact]
    public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
    {
        // Arrange
        var existingInstance = new Pipeline<object?, object?>(new[] { new MyFeature() });

        // Act
        var builder = new PipelineBuilder<object?, object?>(existingInstance);

        // Assert
        builder.Features.Should().BeEquivalentTo(existingInstance.Features);
    }

    [Fact]
    public void Can_Process_Pipeline_With_Feature()
    {
        // Arrange
        PipelineContext<object?, object?>? context = null;
        Action<PipelineContext<object?, object?>> processCallback = new(ctx => { context = ctx; });
        var sut = new PipelineBuilder<object?, object?>()
            .AddFeature(new MyFeature(processCallback))
            .Build();

        // Act
        sut.Process(model: 1, context: 2);

        // Assert
        context.Should().NotBeNull();
        context!.Model.Should().BeEquivalentTo(1);
        context.Context.Should().BeEquivalentTo(2);
    }

    private sealed class MyFeature : IPipelineFeature<object?, object?>
    {
        private readonly Action<PipelineContext<object?, object?>> _processCallback;

        public MyFeature()
            => _processCallback = new Action<PipelineContext<object?, object?>>(_ => { });

        public MyFeature(Action<PipelineContext<object?, object?>> processCallback)
            => _processCallback = processCallback;

        public void Process(PipelineContext<object?, object?> context)
            => _processCallback.Invoke(context);
    }

    private sealed class MyReplacedFeature : IPipelineFeature<object?, object?>
    {
        public void Process(PipelineContext<object?, object?> context)
        {
            throw new NotImplementedException();
        }
    }
}
