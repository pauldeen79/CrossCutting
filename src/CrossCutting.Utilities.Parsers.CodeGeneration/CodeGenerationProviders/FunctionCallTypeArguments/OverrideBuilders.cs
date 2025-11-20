namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.UtilitiesParsersBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetBuildersAsync(
            GetOverrideModelsAsync(typeof(IFunctionCallTypeArgumentBase)),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsersFunctionCallTypeArguments);
}
