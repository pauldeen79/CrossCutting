namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(ICommandService commandService) : QueryEvaluatorCSharpClassBase(commandService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken) => GetBuilderExtensionsAsync(GetAbstractionsInterfacesAsync(), $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions.Builders", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions", CurrentNamespace);

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
