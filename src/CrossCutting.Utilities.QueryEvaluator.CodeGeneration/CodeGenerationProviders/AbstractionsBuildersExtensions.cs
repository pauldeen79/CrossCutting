namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderExtensionsAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders", "CrossCutting.Utilities.QueryEvaluator.Abstractions", "CrossCutting.Utilities.QueryEvaluator.Builders.Extensions");

    public override string Path => "CrossCutting.Utilities.QueryEvaluator/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
