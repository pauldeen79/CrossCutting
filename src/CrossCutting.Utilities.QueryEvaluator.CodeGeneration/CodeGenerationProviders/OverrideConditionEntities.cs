namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideConditionEntities(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}/Conditions";

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IConditionBase), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");
    //This is kind of a hack, because the name ToTypedBuilder is already taken. So we need to use a different name.
    protected override string ToTypedBuilderFormatString => "ToTypedBuilderCore";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IConditionBase)), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core.Conditions");
}
