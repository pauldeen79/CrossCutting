namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideEvaluatableBuilders(ICommandService commandService) : ExpressionEvaluatorCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders/Evaluatables";

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync()
        => CreateBaseClassAsync(typeof(IEvaluatableBase), Constants.Namespaces.UtilitiesExpressionEvaluator);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IEvaluatableBase)),
            $"{Constants.Namespaces.UtilitiesExpressionEvaluator}.Builders.Evaluatables",
            $"{Constants.Namespaces.UtilitiesExpressionEvaluator}.Evaluatables");
}
