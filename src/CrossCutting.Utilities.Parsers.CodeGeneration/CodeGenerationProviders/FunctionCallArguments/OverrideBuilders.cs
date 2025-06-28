namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders.FunctionCallArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallArgumentBase), Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.UtilitiesParsersBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IFunctionCallArgumentBase)),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsersFunctionCallArguments);
}
