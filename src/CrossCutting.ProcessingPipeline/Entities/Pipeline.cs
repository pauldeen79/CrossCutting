namespace CrossCutting.ProcessingPipeline.Entities;

public class Pipeline : PipelineBase
{
    public Pipeline(IEnumerable<IPipelineFeature> features) : base(features)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }
}

