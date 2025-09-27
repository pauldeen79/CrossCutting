namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuildersExpressions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Expressions";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetNonCoreModelsAsync(typeof(IPropertyNameExpression).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders.Expressions", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Expressions");

    protected override bool CreateAsObservable => true;
}
