namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArguments;

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesExpressionEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetEntities(GetOverrideModels(typeof(IFunctionCallTypeArgumentBase)), CurrentNamespace);
}
