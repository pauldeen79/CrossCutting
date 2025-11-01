namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ExpressionEvaluatorCSharpClassBase(ICommandService commandService) : CsharpClassGeneratorPipelineCodeGenerationProviderBase(commandService)
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    protected override string ProjectName => Constants.ProjectName;
    protected override string CoreNamespace => Constants.Namespaces.UtilitiesExpressionEvaluator; // standard implementation thinks we're using the project name concatenated with '.Domain'
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

    // Skip builder pattern on abstractions (Most importantly, IOperator, because we generate them manually. But also on IParseResult, which is only used for removing code duplication on parse results)
    protected override bool UseBuilderAbstractionsTypeConversion => false;
}
