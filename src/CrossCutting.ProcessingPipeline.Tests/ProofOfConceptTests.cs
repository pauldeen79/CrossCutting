namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    [Fact]
    public void Can_Create_Pipeline()
    {
        // Arrange
        var builder = new PipelineBuilder();

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
        var builder = new PipelineBuilder();

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
        var builder = new PipelineBuilder();

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
        var builder = new PipelineBuilder()
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
        var builder = new PipelineBuilder { Features = null! };

        // Act
        var validationResults = builder.Validate(new(builder));

        // Assert
        validationResults.Should().ContainSingle();
    }

    [Fact]
    public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
    {
        // Arrange
        var existingInstance = new Pipeline(new[] { new MyFeature() });

        // Act
        var builder = new PipelineBuilder(existingInstance);

        // Assert
        builder.Features.Should().BeEquivalentTo(existingInstance.Features);
    }

    private class MyFeature : IPipelineFeature
    {
    }

    private class MyReplacedFeature : IPipelineFeature
    {
    }
}
