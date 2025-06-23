namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetEntityInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.QueryEvaluator", "CrossCutting.Utilities.QueryEvaluator.Abstractions");

    public override string Path => "CrossCutting.Utilities.QueryEvaluator/Abstractions";

    protected override bool EnableEntityInheritance => true;
}
