namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class OverrideEntities : CrossCuttingCSharpClassBase
{
    public override string Path => CrossCutting.CodeGeneration.Constants.Paths.FunctionParseResultArguments;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(IFunctionParseResultArgument), CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsers);

    public override object CreateModel()
        => GetImmutableClasses(GetOverrideModels(typeof(IFunctionParseResultArgument)), CurrentNamespace);
}
