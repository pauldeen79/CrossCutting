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
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue, new Literal($"{typeof(CultureInfo).FullName}.{nameof(CultureInfo.InvariantCulture)}"))
            );
    }
}
