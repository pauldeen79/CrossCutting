namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class AbstractNonGenericBuilders : CrossCuttingCSharpClassBase
{
    public override string Path => Constants.Namespaces.UtilitiesParsersBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override string FileNameSuffix => ".nongeneric.template.generated";

    public override object CreateModel()
        => GetImmutableNonGenericBuilderClasses(
            GetAbstractModels(),
            Constants.Namespaces.UtilitiesParsers,
            Constants.Namespaces.UtilitiesParsersBuilders);
}
