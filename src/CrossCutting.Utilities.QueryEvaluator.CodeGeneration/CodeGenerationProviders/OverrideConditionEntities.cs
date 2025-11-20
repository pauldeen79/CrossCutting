namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideConditionEntities(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Conditions";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IConditionBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IConditionBase)), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Conditions");
}
