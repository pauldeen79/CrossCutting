namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionResolver
{
    Result<FunctionAndTypeDescriptor> Resolve(FunctionCallContext functionCallContext);
}
