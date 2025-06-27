namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideOperatorEntities(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Operators";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IOperator), Constants.Namespaces.UtilitiesQueryEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IOperator)), CurrentNamespace);
}
