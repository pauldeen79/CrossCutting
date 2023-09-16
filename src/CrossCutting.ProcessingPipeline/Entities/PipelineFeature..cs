using CrossCutting.ProcessingPipeline.Builders;

namespace CrossCutting.ProcessingPipeline.Entities;

public abstract record PipelineFeature : PipelineFeatureBase
{
    protected PipelineFeature(PipelineFeature original) : base(original)
    {
    }

    protected PipelineFeature(int order) : base(order)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public abstract PipelineFeatureBuilder ToBuilder();
}
