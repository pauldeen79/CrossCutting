namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreBuilders : CrossCuttingCSharpClassBase
{
    public override string Path => Constants.Namespaces.UtilitiesParsersBuilders;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetCoreModels(),
            Constants.Namespaces.UtilitiesParsers,
            Constants.Namespaces.UtilitiesParsersBuilders);
}
