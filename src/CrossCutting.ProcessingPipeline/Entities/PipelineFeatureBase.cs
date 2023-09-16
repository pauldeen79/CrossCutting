namespace CrossCutting.ProcessingPipeline.Entities;

public partial record PipelineFeatureBase
{
    public int Order
    {
        get;
        private set;
    }

    public PipelineFeatureBase(int order)
    {
        Order = order;
    }
}
