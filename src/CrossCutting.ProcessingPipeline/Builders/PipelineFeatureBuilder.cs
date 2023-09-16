namespace CrossCutting.ProcessingPipeline.Builders;

public abstract class PipelineFeatureBuilder
{
    public int Order
    {
        get
        {
            return _orderDelegate.Value;
        }
        set
        {
            _orderDelegate = new (() => value);
        }
    }

    public abstract Entities.PipelineFeature Build();

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }

    public PipelineFeatureBuilder WithOrder(int order)
    {
        Order = order;
        return this;
    }

    public PipelineFeatureBuilder WithOrder(Func<int> orderDelegate)
    {
        _orderDelegate = new (orderDelegate);
        return this;
    }

    protected PipelineFeatureBuilder()
    {
        _orderDelegate = new (() => default(int));
    }

    protected PipelineFeatureBuilder(Entities.PipelineFeature source)
    {
        _orderDelegate = new (() => source.Order);
    }

    private Lazy<int> _orderDelegate;
}
