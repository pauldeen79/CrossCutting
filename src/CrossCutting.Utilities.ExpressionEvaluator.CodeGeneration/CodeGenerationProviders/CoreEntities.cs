namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Namespaces.UtilitiesExpressionEvaluator;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetCoreModelsAsync(), CurrentNamespace);
}
