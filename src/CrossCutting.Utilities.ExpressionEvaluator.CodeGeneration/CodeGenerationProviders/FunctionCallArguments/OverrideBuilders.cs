namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders.FunctionCallArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(IFunctionCallArgumentBase), Constants.Namespaces.UtilitiesExpressionEvaluator);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.UtilitiesExpressionEvaluatorBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilders(
            GetOverrideModels(typeof(IFunctionCallArgumentBase)),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesExpressionEvaluatorFunctionCallArguments);
}
