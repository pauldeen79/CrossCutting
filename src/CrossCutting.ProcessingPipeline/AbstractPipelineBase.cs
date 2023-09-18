namespace CrossCutting.ProcessingPipeline;

public class AbstractPipelineBase<T>
{
    [Required]
    public IReadOnlyCollection<T> Features { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AbstractPipelineBase(IEnumerable<T> features)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        Features = features == null
            ? null
            : new ReadOnlyCollection<T>(features.ToList());
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
