namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArguments;

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesParsers);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IFunctionCallTypeArgumentBase)), CurrentNamespace);
}
