namespace CrossCutting.ProcessingPipeline.Entities;

public class ProcessingPipeline : ProcessingPipelineBase
{
    public ProcessingPipeline(IEnumerable<PipelineFeature> features) : base(features)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }
}

