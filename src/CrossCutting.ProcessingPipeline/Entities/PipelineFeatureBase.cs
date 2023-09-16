namespace CrossCutting.ProcessingPipeline.Entities;

public class PipelineFeatureBase
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
