namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    protected override bool EnableBuilderInhericance => true;
    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;

    // Do not generate 'With' methods. Do this on the interfaces instead.
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string AddMethodNameFormatString => string.Empty;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetAbstractModelsAsync(), CurrentNamespace, Constants.Namespaces.UtilitiesParsers);
}
