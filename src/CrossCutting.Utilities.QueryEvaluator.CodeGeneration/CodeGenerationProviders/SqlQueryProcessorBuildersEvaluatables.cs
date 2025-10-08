namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class SqlQueryProcessorBuildersEvaluatables(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql/Evaluatables/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEvaluatableBuildersAsync(GetNonCoreModelsAsync(typeof(ISqlLikeEvaluatable).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Evaluatables.Builders", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.QueryProcessors.Sql.Evaluatables");
    
    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;

    protected override Task<Result<TypeBase>> GetBaseClassAsync()
        => Task.FromResult(Result.Success(GetEvaluatableBase()));
}
