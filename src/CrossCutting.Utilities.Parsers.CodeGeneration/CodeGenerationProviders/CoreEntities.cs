namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetCoreModelsAsync(), CurrentNamespace);
}
