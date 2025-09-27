namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class SqlQueryProcessorExpressions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql/Expressions";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetNonCoreModelsAsync(typeof(ISqlLikeExpression).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Expressions");
}
