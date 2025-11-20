namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuildersEvaluatables(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Evaluatables";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetBuildersAsync(GetNonCoreModelsAsync(typeof(IPropertyNameEvaluatable).FullName.GetNamespaceWithDefault()), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders.Evaluatables", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Evaluatables");

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;

    protected override Task<Result<TypeBase>> GetBaseClassAsync()
        => Task.FromResult(Result.Success(GetEvaluatableBase()));
}
