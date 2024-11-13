namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class AbstractBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(await GetAbstractModels(), CurrentNamespace, Constants.Namespaces.UtilitiesParsers);
}
