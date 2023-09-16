namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    [Fact]
    public void Can_Create_Pipeline()
    {
        // Arrange
        var builder = new ProcessingPipelineBuilder();

        // Act
        var pipeline = builder.AddFeatures(new MyFeature(1)).Build();

        // Assert
        pipeline.Features.Should().ContainSingle();
        pipeline.Features.Single().Should().BeOfType<MyFeature>();
    }

    private class MyFeature : PipelineFeature
    {
        public MyFeature(int order) : base(order)
        {
        }
    }
}
