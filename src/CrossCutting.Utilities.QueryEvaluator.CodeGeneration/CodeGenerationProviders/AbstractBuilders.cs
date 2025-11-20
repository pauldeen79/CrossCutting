namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders";

    protected override bool EnableBuilderInhericance => true;
    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetBuildersAsync(GetAbstractModelsAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");
}
