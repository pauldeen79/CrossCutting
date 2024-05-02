namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreEntities : CrossCuttingCSharpClassBase
{
    public CoreEntities(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntities(await GetCoreModels(), Constants.Namespaces.UtilitiesParsers);
}
