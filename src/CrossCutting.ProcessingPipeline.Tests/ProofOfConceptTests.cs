namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    [Fact]
    public void Can_Create_Pipeline()
    {
        // Arrange
        var builder = new ProcessingPipelineBuilder();

        // Act
        var pipeline = builder.AddFeatures(new MyFeature()).Build();

        // Assert
        pipeline.Features.Should().ContainSingle();
        pipeline.Features.Single().Should().BeOfType<MyFeatureImplementation>();
    }

    private sealed class MyFeature : PipelineFeatureBuilder
    {
        public override PipelineFeature Build() => new MyFeatureImplementation(Order);
    }

    private sealed record MyFeatureImplementation : PipelineFeature
    {
        public MyFeatureImplementation(PipelineFeature original) : base(original)
        {
        }

        public MyFeatureImplementation(int order) : base(order)
        {
        }

        public override PipelineFeatureBuilder ToBuilder() => new MyFeature();
    }
}
