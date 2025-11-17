namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders(ICommandService commandService) : ExpressionEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders";

    protected override bool EnableBuilderInhericance => true;
    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
    {
        return GetBuildersAsync(GetAbstractModelsAsync(), CurrentNamespace, Constants.Namespaces.UtilitiesExpressionEvaluator);
    }
}
