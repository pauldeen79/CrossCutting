namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderExtensionsAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions");

    public override string Path => "CrossCutting.Utilities.ExpressionEvaluator/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
