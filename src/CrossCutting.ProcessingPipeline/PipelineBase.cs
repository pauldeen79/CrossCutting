namespace CrossCutting.ProcessingPipeline;

public class PipelineBase<TModel, TContext>
{
    [Required]
    public IReadOnlyCollection<IPipelineFeature<TModel, TContext>> Features { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PipelineBase(IEnumerable<IPipelineFeature<TModel, TContext>> features)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        Features = features == null
            ? null
            : new ReadOnlyCollection<IPipelineFeature<TModel, TContext>>(features.ToList());
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
