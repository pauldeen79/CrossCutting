namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryEntities(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Queries";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IQueryBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IQueryBase)), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Queries");
}
