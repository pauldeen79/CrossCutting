namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class CrossCuttingCSharpClassBase(IPipelineService pipelineService) : CsharpClassGeneratorPipelineCodeGenerationProviderBase(pipelineService)
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    protected override string ProjectName => Constants.ProjectName;
    protected override string CoreNamespace => Constants.Namespaces.UtilitiesParsers; // standard implementation thinks we're using the project name concatenated with '.Domain'
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool CreateRecord => true;
    protected override bool GenerateMultipleFiles => false;
    protected override bool EnableGlobalUsings => true;
    protected override bool AddImplicitOperatorOnBuilder => true;
    protected override bool UseBuilderAbstractionsTypeConversion => true;
    protected override string[] GetBuilderAbstractionsTypeConversionNamespaces() => [ $"{CoreNamespace}.Abstractions" ];

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

        //// Hacking here...
        //// Types in Abstractions don't get converted to builders out of the box, not sure why
        //var type = typeof(Models.Abstractions.IFunctionCallArgument);
        //yield return new TypenameMappingBuilder()
        //    .WithSourceTypeName($"{CoreNamespace}.Abstractions.I{type.GetEntityClassName()}")
        //    .WithTargetTypeName($"{CoreNamespace}.Abstractions.I{type.GetEntityClassName()}")
        //    .AddMetadata
        //    (
        //        new MetadataBuilder().WithValue($"{CoreNamespace}.Builders.Abstractions").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
        //        new MetadataBuilder().WithValue("{ClassName($property.TypeName)}Builder").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
        //        new MetadataBuilder().WithValue($"{CoreNamespace}.Builders.Abstractions").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
        //        new MetadataBuilder().WithValue("{NoGenerics(ClassName($property.TypeName))}Builder{GenericArguments(ClassName($property.TypeName), true)}").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceName),
        //        new MetadataBuilder().WithValue("[Name][NullableSuffix].ToBuilder()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression),
        //        new MetadataBuilder().WithValue(new Literal($"default({CoreNamespace}.Builders.Abstractions.{{NoGenerics(ClassName($property.TypeName))}}Builder{{GenericArguments(ClassName($property.TypeName), true)}})", null)).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue),
        //        new MetadataBuilder().WithValue("[Name][NullableSuffix].Build()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression),
        //        new MetadataBuilder().WithName(ClassFramework.Pipelines.MetadataNames.CustomEntityInterfaceTypeName).WithValue($"{CoreNamespace}.Abstractions.I{type.GetEntityClassName()}")
        //    );
    }
}
