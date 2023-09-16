namespace CrossCutting.ProcessingPipeline.Entities;

public partial record ProcessingPipeline : ProcessingPipelineBase
{
    public ProcessingPipeline(ProcessingPipeline original) : base(original)
    {
    }

    public ProcessingPipeline(IEnumerable<PipelineFeature> features) : base(features)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }
}

