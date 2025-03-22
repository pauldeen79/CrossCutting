namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesExpressionEvaluator);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.UtilitiesExpressionEvaluatorBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilders(
            GetOverrideModels(typeof(IFunctionCallTypeArgumentBase)),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesExpressionEvaluatorFunctionCallTypeArguments);
}
