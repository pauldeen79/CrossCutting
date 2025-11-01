namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetCoreModelsAsync(), CurrentNamespace, Constants.Namespaces.UtilitiesParsers);

    protected override bool CreateAsObservable => true;
}
