namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderExtensionsAsync(GetAbstractionsInterfacesAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Builders.Abstractions", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions", CurrentNamespace);

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
