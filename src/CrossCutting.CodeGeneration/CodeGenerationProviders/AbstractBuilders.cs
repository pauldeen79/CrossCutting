namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class AbstractBuilders : CrossCuttingCSharpClassBase
{
    public override string Path => Constants.Namespaces.UtilitiesParsersBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetAbstractModels(),
            Constants.Namespaces.UtilitiesParsers,
            Constants.Namespaces.UtilitiesParsersBuilders);
}
