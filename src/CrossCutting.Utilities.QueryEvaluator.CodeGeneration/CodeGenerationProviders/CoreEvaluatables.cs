namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEvaluatables(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Evaluatables";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetNonCoreModelsAsync(typeof(IPropertyNameEvaluatable).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Evaluatables");

    protected override bool EnableEntityInheritance => true;

    protected override Task<Result<TypeBase>> GetBaseClassAsync()
        => Task.FromResult(Result.Success(GetEvaluatableBase()));
}
