﻿namespace CrossCutting.ProcessingPipeline;

public abstract class AbstractPipelineBase<T>
{
    [Required]
    [ValidateObject]
    public IReadOnlyCollection<T> Components { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected AbstractPipelineBase(IEnumerable<T>? features)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        Components = features == null
            ? null
            : new ReadOnlyCollection<T>(features.ToList());
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
