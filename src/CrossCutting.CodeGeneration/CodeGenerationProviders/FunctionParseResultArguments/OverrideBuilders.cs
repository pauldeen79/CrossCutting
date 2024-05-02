namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class OverrideBuilders : CrossCuttingCSharpClassBase
{
    public OverrideBuilders(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => CrossCutting.CodeGeneration.Constants.Paths.FunctionParseResultArgumentBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override async Task<TypeBase?> GetBaseClass() => await CreateBaseClass(typeof(IFunctionParseResultArgument), CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsers);
    protected override string BaseClassBuilderNamespace => CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuilders;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(
            await GetOverrideModels(typeof(IFunctionParseResultArgument)),
            CurrentNamespace,
            CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersFunctionParseResultArguments);
}
