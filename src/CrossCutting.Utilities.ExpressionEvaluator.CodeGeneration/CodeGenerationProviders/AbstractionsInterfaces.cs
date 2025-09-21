namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetEntityInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.ExpressionEvaluator", "CrossCutting.Utilities.ExpressionEvaluator.Abstractions");

    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Abstractions";

    protected override bool EnableEntityInheritance => true;
}
