namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class QueryEvaluatorCSharpClassBase(IPipelineService pipelineService) : CsharpClassGeneratorPipelineCodeGenerationProviderBase(pipelineService)
{
    public IEnumerable<TypenameMappingBuilder> GetTypenameMappings()
        => CreateTypenameMappings();

    public IEnumerable<NamespaceMappingBuilder> GetNamespaceMappings()
        => CreateNamespaceMappings();

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    protected override string ProjectName => Constants.ProjectName;
    protected override string BuilderAbstractionsNamespace => $"{ProjectName}.Abstractions.Builders";
    protected override string AbstractionsNamespace => $"{ProjectName}.Abstractions";
    protected override string DomainsNamespace => $"{ProjectName}.Abstractions.Domains";
    protected override string ValidationNamespace => $"{ProjectName}.Abstractions.Validation";
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool CreateRecord => true;
    protected override bool GenerateMultipleFiles => false;
    protected override bool EnableGlobalUsings => true;

    protected override IEnumerable<TypenameMappingBuilder> GetAdditionalTypenameMappings()
    {
        yield return new TypenameMappingBuilder(typeof(IFormatProvider))
            .AddMetadata
            (
                new MetadataBuilder(MetadataNames.CustomBuilderDefaultValue, new Literal($"{typeof(CultureInfo).FullName}.{nameof(CultureInfo.InvariantCulture)}"))
            );

        var expressionType = typeof(IExpression);
        foreach (var mapping in CreateBuilderAbstractionTypeConversionTypenameMappings(expressionType.GetEntityClassName(), expressionType.GetGenericTypeArgumentsString(), "CrossCutting.Utilities.ExpressionEvaluator.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator"))
        {
            yield return mapping;
        }
    }

    private static TypenameMappingBuilder[] CreateBuilderAbstractionTypeConversionTypenameMappings(
        string entityClassName,
        string genericTypeArgumentsString,
        string abstractionsNamespace,
        string builderAbstractionsNamespace,
        string coreNamespace)
        => [
            new TypenameMappingBuilder($"{abstractionsNamespace}.I{entityClassName}{genericTypeArgumentsString}", $"{abstractionsNamespace}.I{entityClassName}")
                .AddMetadata
                (
                    new MetadataBuilder(MetadataNames.CustomBuilderNamespace, builderAbstractionsNamespace),
                    new MetadataBuilder(MetadataNames.CustomBuilderName, $"I{entityClassName.WithoutGenerics()}Builder"),
                    new MetadataBuilder(MetadataNames.CustomBuilderInterfaceNamespace, builderAbstractionsNamespace),
                    new MetadataBuilder(MetadataNames.CustomBuilderInterfaceName, $"I{entityClassName.WithoutGenerics()}Builder{genericTypeArgumentsString}"),
                    new MetadataBuilder(MetadataNames.CustomBuilderInterfaceTypeName, $"{builderAbstractionsNamespace}.I{entityClassName.WithoutGenerics()}Builder{genericTypeArgumentsString}"),
                    new MetadataBuilder(MetadataNames.CustomBuilderSourceExpression, "[Name][NullableSuffix].ToBuilder()[ForcedNullableSuffix]"),
                    new MetadataBuilder(MetadataNames.CustomBuilderDefaultValue, new Literal($"default({builderAbstractionsNamespace}.I{entityClassName.WithoutGenerics()}Builder{genericTypeArgumentsString})")),
                    new MetadataBuilder(MetadataNames.CustomBuilderMethodParameterExpression, "[Name][NullableSuffix].Build()[ForcedNullableSuffix]"),
                    new MetadataBuilder(MetadataNames.CustomEntityInterfaceTypeName, $"{abstractionsNamespace}.I{entityClassName}")
                ),
            new TypenameMappingBuilder($"{coreNamespace}.Builders.I{entityClassName}Builder", $"{builderAbstractionsNamespace}.I{entityClassName}Builder"),
            new TypenameMappingBuilder($"{coreNamespace}.Abstractions.{entityClassName}", $"{abstractionsNamespace}.I{entityClassName}"),
            new TypenameMappingBuilder($"{abstractionsNamespace}.{entityClassName}", $"{abstractionsNamespace}.I{entityClassName}")
        ];
}
