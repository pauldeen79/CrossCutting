namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => CrossCutting.CodeGeneration.Constants.Paths.FunctionCallArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(IFunctionCallArgument), CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilders(
            GetOverrideModels(typeof(IFunctionCallArgument)),
            CurrentNamespace,
            CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersFunctionCallArguments);
}
