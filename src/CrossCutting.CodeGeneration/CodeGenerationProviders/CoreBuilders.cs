namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreBuilders : CrossCuttingCSharpClassBase
{
    public CoreBuilders(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(
            await GetCoreModels(),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsers);
}
