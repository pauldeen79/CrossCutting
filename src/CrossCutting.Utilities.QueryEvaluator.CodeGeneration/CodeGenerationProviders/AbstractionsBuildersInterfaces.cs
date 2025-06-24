namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders", "CrossCutting.Utilities.QueryEvaluator.Abstractions", "CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders");

    public override string Path => "CrossCutting.Utilities.QueryEvaluator/Abstractions/Builders";
    
    protected override bool EnableEntityInheritance => true;
}
