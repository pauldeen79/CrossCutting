namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderExtensionsAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions");

    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
