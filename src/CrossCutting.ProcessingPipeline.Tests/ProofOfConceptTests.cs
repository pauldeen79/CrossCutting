namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    [Fact]
    public void Can_Create_Pipeline()
    {
        // Arrange
        var builder = new ProcessingPipelineBuilder();

        // Act
        var pipeline = builder
            .AddFeature(new MyFeature())
            .Build();

        // Assert
        pipeline.Features.Should().ContainSingle();
        pipeline.Features.Single().Should().BeOfType<MyFeature>();
    }

    private class MyFeature : IPipelineFeature
    {
        public MyFeature()
        {
        }
    }
}
