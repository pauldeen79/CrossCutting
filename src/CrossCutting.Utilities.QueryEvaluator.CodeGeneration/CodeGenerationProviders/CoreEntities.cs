namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => Constants.Namespaces.UtilitiesQueryEvaluator;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetCoreModelsAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");
}
