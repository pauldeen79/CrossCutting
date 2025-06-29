namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Builders.Abstractions", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Builders.Abstractions");

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Abstractions";
    
    protected override bool EnableEntityInheritance => true;
}
