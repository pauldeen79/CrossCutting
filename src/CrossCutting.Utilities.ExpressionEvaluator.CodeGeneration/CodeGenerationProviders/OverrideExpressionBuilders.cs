namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideExpressionBuilders(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders/Expressions";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IExpressionBase), Constants.Namespaces.UtilitiesExpressionEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IExpressionBase)),
            $"{Constants.Namespaces.UtilitiesExpressionEvaluator}.Builders.Expressions",
            $"{Constants.Namespaces.UtilitiesExpressionEvaluator}.Expressions");
}
