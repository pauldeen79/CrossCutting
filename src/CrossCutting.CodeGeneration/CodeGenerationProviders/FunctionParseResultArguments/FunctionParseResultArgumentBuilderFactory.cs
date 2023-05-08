namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class FunctionParseResultArgumentBuilderFactory : CrossCuttingCSharpClassBase
{
    public override string Path => CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuilders;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(IFunctionParseResultArgument)),
            new(
                CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuilders,
                nameof(FunctionParseResultArgumentBuilderFactory),
                CrossCutting.CodeGeneration.Constants.TypeNames.FunctionParseResultArgument,
                CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersBuildersFunctionParseResultArguments,
                CrossCutting.CodeGeneration.Constants.Types.FunctionParseResultArgumentBuilder,
                CrossCutting.CodeGeneration.Constants.Namespaces.UtilitiesParsersFunctionParseResultArguments
            )
        );
}
