namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.UtilitiesParsersBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IFunctionCallTypeArgumentBase)),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsersFunctionCallTypeArguments);
}
