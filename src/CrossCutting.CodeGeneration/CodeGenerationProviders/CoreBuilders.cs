namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(await GetCoreModels(), CurrentNamespace, Constants.Namespaces.UtilitiesParsers);

    protected override bool CreateAsObservable => true;
}
