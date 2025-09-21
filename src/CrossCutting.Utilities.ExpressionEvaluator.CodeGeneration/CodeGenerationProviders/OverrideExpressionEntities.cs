namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideExpressionEntities(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Expressions";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IExpressionBase), Constants.Namespaces.UtilitiesExpressionEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IExpressionBase)), $"{Constants.Namespaces.UtilitiesExpressionEvaluator}.Expressions");
}
