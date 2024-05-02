namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class CrossCuttingCSharpClassBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    protected CrossCuttingCSharpClassBase(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    protected override string ProjectName => Constants.ProjectName;
    protected override string CoreNamespace => Constants.Namespaces.UtilitiesParsers; // standard implementation thinks we're using the project name concatenated with '.Domain'
    protected override bool CopyAttributes => true;
    protected override bool CreateRecord => true;

    protected override bool IsAbstractType(Type type)
    {
        type = type.IsNotNull(nameof(type));

        if (type.IsInterface && type.Namespace == $"{CodeGenerationRootNamespace}.Models" && type.Name.Substring(1) == Constants.Types.FunctionParseResultArgument)
        {
            return true;
        }
        return base.IsAbstractType(type);
    }

    protected override IEnumerable<TypenameMappingBuilder> CreateAdditionalTypenameMappings()
    {
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
