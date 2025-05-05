namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberResolver
{
    Result<MemberAndTypeDescriptor> Resolve(FunctionCallContext functionCallContext);
}
