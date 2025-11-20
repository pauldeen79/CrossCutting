namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractEntities(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override string Path => Constants.Namespaces.UtilitiesQueryEvaluator;

    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.None; // not needed for abstract entities, because each derived class will do its own validation

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetAbstractModelsAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Core");
}
