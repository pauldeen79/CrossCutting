namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryBuilders(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Queries";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IQueryBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IQueryBase)),
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders.Queries",
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Queries");
}
