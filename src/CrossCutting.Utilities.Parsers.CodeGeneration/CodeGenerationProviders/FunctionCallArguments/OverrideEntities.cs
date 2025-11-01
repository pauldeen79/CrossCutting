namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders.FunctionCallArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => Constants.Paths.FunctionCallArguments;

    protected override bool EnableEntityInheritance => true;
    protected override Task<Result<TypeBase>> GetBaseClassAsync() => CreateBaseClassAsync(typeof(IFunctionCallArgumentBase), Constants.Namespaces.UtilitiesParsers);

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetOverrideModelsAsync(typeof(IFunctionCallArgumentBase)), CurrentNamespace);
}
