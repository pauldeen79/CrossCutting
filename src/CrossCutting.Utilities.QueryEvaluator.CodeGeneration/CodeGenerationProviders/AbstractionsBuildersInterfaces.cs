namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), CurrentNamespace, $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions", CurrentNamespace);

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions/Builders";
    
    protected override bool EnableEntityInheritance => true;
}
