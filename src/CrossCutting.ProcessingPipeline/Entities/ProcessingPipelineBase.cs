namespace CrossCutting.ProcessingPipeline.Entities;

public partial record ProcessingPipelineBase
{
    [Required]
    public IReadOnlyCollection<PipelineFeature> Features { get; }

    public ProcessingPipelineBase(IEnumerable<PipelineFeature> features)
    {
        features = ArgumentGuard.IsNotNull(features, nameof(features));

        Features = new ValueCollection<PipelineFeature>(features);
    }
}
