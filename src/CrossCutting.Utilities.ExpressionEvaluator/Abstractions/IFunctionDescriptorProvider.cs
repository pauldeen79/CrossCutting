namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionDescriptorProvider
{
    IReadOnlyCollection<FunctionDescriptor> GetAll();
}
