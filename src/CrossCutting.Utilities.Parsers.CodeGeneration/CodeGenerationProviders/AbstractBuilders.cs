namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    protected override bool EnableBuilderInhericance => true;
    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetAbstractModelsAsync(), CurrentNamespace, Constants.Namespaces.UtilitiesParsers);
}
