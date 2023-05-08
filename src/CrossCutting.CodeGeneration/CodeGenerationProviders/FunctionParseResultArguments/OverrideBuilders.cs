namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders : CrossCuttingCSharpClassBase
{
    public override string Path => CrossCutting.CodeGeneration.Constants.Paths.FunctionParseResultArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(IFunctionParseResultArgument), CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuilders;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetOverrideModels(typeof(IFunctionParseResultArgument)),
            CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersFunctionParseResultArguments,
            CurrentNamespace);
}
