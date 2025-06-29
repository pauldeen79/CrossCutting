namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetEntityInterfacesAsync(GetAbstractionsInterfacesAsync(), Constants.Namespaces.UtilitiesQueryEvaluator, $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions");

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Abstractions";

    protected override bool EnableEntityInheritance => true;
}
