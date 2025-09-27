namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class SqlQueryProcessorEvaluatables(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql/Evaluatables";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetNonCoreModelsAsync(typeof(ISqlLikeEvaluatable).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Evaluatables");
}
