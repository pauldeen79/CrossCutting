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
    protected override string CoreNamespace => Constants.Namespaces.UtilitiesQueryEvaluator; // standard implementation thinks we're using the project name concatenated with '.Core'
    protected override string BuilderAbstractionsNamespace => $"{ProjectName}.Abstractions.Builders";
    //protected override string AbstractionsParentNamespace => ProjectName;
    //protected override bool InheritFromInterfaces => true;
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool CreateRecord => true;
    protected override bool GenerateMultipleFiles => false;
    protected override bool EnableGlobalUsings => true;

    protected override IEnumerable<TypenameMappingBuilder> CreateAdditionalTypenameMappings()
    {
        // Map validation attributes to Abstractions instead of main assembly
        foreach (var validationType in GetType().Assembly.GetTypes().Where(x => x.Namespace == $"{CodeGenerationRootNamespace}.Validation"))
        {
            yield return new TypenameMappingBuilder()
                .WithSourceType(validationType)
                .WithTargetTypeName(validationType.FullName!.Replace(CodeGenerationRootNamespace, $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions"));
        }

        // Map domains (enumerations and value type) to Abstractions instead of main assembly
        foreach (var domainsType in GetType().Assembly.GetTypes().Where(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models.Domains"))
        {
            yield return new TypenameMappingBuilder()
                .WithSourceType(domainsType)
                .WithTargetTypeName(domainsType.FullName!.Replace($"{CodeGenerationRootNamespace}.Models", $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions"));
        }

        yield return new TypenameMappingBuilder()
            .WithSourceType(typeof(IFormatProvider))
            .WithTargetType(typeof(IFormatProvider))
            .AddMetadata
            (
                new MetadataBuilder()
                    .WithValue(new Literal($"{typeof(CultureInfo).FullName}.{nameof(CultureInfo.InvariantCulture)}", null))
                    .WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue)
            );
    }
}
