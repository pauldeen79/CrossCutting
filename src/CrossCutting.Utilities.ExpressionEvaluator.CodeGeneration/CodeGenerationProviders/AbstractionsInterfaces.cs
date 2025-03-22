namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => "CrossCutting.Utilities.ExpressionEvaluator/Abstractions";

    protected override bool EnableEntityInheritance => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetEntityInterfaces(GetAbstractionsInterfaces(), "CrossCutting.Utilities.ExpressionEvaluator", "CrossCutting.Utilities.ExpressionEvaluator.Abstractions");
}
