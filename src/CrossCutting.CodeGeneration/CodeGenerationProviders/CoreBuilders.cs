namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreBuilders : CrossCuttingCSharpClassBase
{
    public CoreBuilders(ICsharpExpressionDumper csharpExpressionDumper, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionDumper, builderPipeline, builderExtensionPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => $"{Constants.Namespaces.UtilitiesParsers}/Builders";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(
            await GetCoreModels(),
            CurrentNamespace,
            Constants.Namespaces.UtilitiesParsers);
}
