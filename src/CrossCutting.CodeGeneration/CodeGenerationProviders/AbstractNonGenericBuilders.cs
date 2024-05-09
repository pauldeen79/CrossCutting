namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class AbstractNonGenericBuilders : CrossCuttingCSharpClassBase
{
    public AbstractNonGenericBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool CreateAsObservable => true;
    protected override bool IsAbstract => true;
    protected override string FilenameSuffix => ".nongeneric.template.generated";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetNonGenericBuilders(
            await GetAbstractModels(),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsers);
}
