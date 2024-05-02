namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class AbstractEntities : CrossCuttingCSharpClassBase
{
    public AbstractEntities(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => Constants.Namespaces.UtilitiesParsers;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.None; // not needed for abstract entities, because each derived class will do its own validation

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntities(await GetAbstractModels(), Constants.Namespaces.UtilitiesParsers);
}
