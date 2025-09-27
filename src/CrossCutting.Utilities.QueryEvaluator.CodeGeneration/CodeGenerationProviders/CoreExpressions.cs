namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreExpressions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Expressions";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetNonCoreModelsAsync(typeof(IPropertyNameExpression).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Expressions");
}
