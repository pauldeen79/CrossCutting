namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberResolver
{
    Task<Result<MemberAndTypeDescriptor>> ResolveAsync(FunctionCallContext functionCallContext, CancellationToken token);
}
