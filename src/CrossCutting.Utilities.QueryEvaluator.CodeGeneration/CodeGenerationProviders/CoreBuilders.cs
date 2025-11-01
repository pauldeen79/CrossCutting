namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetCoreModelsAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    protected override bool CreateAsObservable => true;
}
