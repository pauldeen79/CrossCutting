namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders.FunctionCallArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallArguments;

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(IFunctionCallArgumentBase), Constants.Namespaces.UtilitiesExpressionEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetEntities(GetOverrideModels(typeof(IFunctionCallArgumentBase)), CurrentNamespace);
}
