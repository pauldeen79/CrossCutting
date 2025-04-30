namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDynamicDescriptorsProvider
{
    IEnumerable<FunctionDescriptor> GetDescriptors();
}
