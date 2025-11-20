namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetCoreModelsAsync(), CurrentNamespace);
}
