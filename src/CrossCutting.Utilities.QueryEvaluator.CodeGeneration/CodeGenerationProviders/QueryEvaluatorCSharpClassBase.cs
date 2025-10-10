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
            .AddMetadata(MetadataNames.CustomBuilderDefaultValue, new Literal($"{typeof(CultureInfo).FullName}.{nameof(CultureInfo.InvariantCulture)}"));

        // Part 1 to get code generation of evaluatables working
        var evaluatableType = typeof(IEvaluatable);
        foreach (var mapping in CreateBuilderAbstractionTypeConversionTypenameMappings(evaluatableType.GetEntityClassName(), evaluatableType.GetGenericTypeArgumentsString(), "CrossCutting.Utilities.ExpressionEvaluator.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator"))
        {
            yield return mapping;
        }
        yield return new TypenameMappingBuilder("CrossCutting.Utilities.ExpressionEvaluator.EvaluatableBase")
            .AddMetadata(MetadataNames.CustomBuilderBaseClassTypeName, "CrossCutting.Utilities.ExpressionEvaluator.Builders.EvaluatableBaseBuilder")
            .AddMetadata(MetadataNames.CustomBuilderNamespace, "CrossCutting.Utilities.ExpressionEvaluator.Builders");
    }

    // Part 2 to get code generation of evaluatables working
    protected static TypeBase GetEvaluatableBase()
        => new ClassBuilder()
            .WithName("EvaluatableBase")
            .WithNamespace("CrossCutting.Utilities.ExpressionEvaluator")
            .AddInterfaces(typeof(IEvaluatable))
            .Build();
}
