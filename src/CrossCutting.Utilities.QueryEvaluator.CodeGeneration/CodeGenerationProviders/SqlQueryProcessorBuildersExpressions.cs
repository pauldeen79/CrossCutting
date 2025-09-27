namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class SqlQueryProcessorBuildersExpressions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql/Expressions/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetNonCoreModelsAsync(typeof(ISqlLikeExpression).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Expressions.Builders", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Expressions");

    protected override bool CreateAsObservable => true;
}
