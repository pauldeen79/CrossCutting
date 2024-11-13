namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreEntities(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntities(await GetCoreModels(), CurrentNamespace);
}
