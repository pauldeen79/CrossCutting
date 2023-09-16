namespace CrossCutting.ProcessingPipeline.Entities;

public abstract class PipelineFeature : PipelineFeatureBase
{
    protected PipelineFeature(int order) : base(order)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }
}
