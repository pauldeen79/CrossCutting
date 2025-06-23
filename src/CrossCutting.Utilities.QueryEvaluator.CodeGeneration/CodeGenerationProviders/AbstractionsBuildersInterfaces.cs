namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.QueryEvaluator.Builders.Abstractions", "CrossCutting.Utilities.QueryEvaluator.Abstractions", "CrossCutting.Utilities.QueryEvaluator.Builders.Abstractions");

    public override string Path => "CrossCutting.Utilities.QueryEvaluator/Builders/Abstractions";
    
    protected override bool EnableEntityInheritance => true;
}
