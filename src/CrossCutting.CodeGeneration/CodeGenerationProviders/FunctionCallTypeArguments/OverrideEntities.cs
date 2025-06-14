namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionCallTypeArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Paths.FunctionCallTypeArguments;

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallTypeArgumentBase), Constants.Namespaces.UtilitiesParsers);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IFunctionCallTypeArgumentBase)), CurrentNamespace);
}
