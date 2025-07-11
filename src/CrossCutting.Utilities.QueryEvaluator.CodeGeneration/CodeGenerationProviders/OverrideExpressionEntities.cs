namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideExpressionEntities(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Expressions";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IExpressionBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IExpressionBase)), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Expressions");
}
