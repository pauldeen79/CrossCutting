namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryBuilders(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Queries";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IQuery), Constants.Namespaces.UtilitiesQueryEvaluator);
    protected override string BaseClassBuilderNamespace => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IQuery)),
            CurrentNamespace,
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Queries");
}
