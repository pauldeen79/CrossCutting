namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideConditionBuilders(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Conditions";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(ICondition), Constants.Namespaces.UtilitiesQueryEvaluator);
    protected override string BaseClassBuilderNamespace => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(ICondition)),
            CurrentNamespace,
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Conditions");
}
