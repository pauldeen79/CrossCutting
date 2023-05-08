namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreEntities : CrossCuttingCSharpClassBase
{
    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override object CreateModel()
        => GetImmutableClasses(GetCoreModels(), Constants.Namespaces.UtilitiesParsers);
}
