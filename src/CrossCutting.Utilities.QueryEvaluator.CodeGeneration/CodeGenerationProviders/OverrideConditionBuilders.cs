namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideConditionBuilders(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Builders/Conditions";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IConditionBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IConditionBase)),
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Builders.Conditions",
            $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Conditions");
}
