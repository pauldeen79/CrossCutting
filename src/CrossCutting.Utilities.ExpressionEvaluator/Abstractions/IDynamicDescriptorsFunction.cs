namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDynamicDescriptorsFunction : IFunction
{
    IEnumerable<FunctionDescriptor> GetDescriptors();
}
