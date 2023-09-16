namespace CrossCutting.ProcessingPipeline.Entities;

public class ProcessingPipelineBase
{
    [Required]
    public IReadOnlyCollection<PipelineFeature> Features { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ProcessingPipelineBase(IEnumerable<PipelineFeature> features)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        Features = features == null
            ? null
            : new ReadOnlyCollection<PipelineFeature>(features.ToList());
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
