namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class QueryEvaluatorCSharpClassBase(IPipelineService pipelineService) : CsharpClassGeneratorPipelineCodeGenerationProviderBase(pipelineService)
{
    private static readonly Type[] baseTypes = [typeof(Models.IQuery), typeof(Models.ICondition), typeof(Models.IExpression), typeof(Models.IOperator)];

    //private const string TypeNameDotClassNameBuilder = "{NoGenerics(ClassName(property.TypeName))}Builder";

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    protected override string ProjectName => Constants.ProjectName;
    protected override string CoreNamespace => Constants.Namespaces.UtilitiesQueryEvaluator; // standard implementation thinks we're using the project name concatenated with '.Domain'
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool CreateRecord => true;
    protected override bool GenerateMultipleFiles => false;
    protected override bool EnableGlobalUsings => true;

    protected override bool IsAbstractType(Type type) => base.IsAbstractType(type) || baseTypes.Contains(type);

    protected override IEnumerable<TypenameMappingBuilder> CreateAdditionalTypenameMappings()
    {
        yield return new TypenameMappingBuilder()
            .WithSourceType(typeof(ValidGroupsAttribute))
            .WithTargetTypeName(typeof(ValidGroupsAttribute).FullName!.Replace(CodeGenerationRootNamespace, ProjectName));

        yield return new TypenameMappingBuilder()
            .WithSourceType(typeof(IFormatProvider))
            .WithTargetType(typeof(IFormatProvider))
            .AddMetadata
            (
                new MetadataBuilder()
                    .WithValue(new Literal($"{typeof(CultureInfo).FullName}.{nameof(CultureInfo.InvariantCulture)}", null))
                    .WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue)
            );

        //var abstractionTypes = GetType().Assembly.GetTypes()
        //        .Where(x => x.IsInterface
        //            && x.Namespace == $"{CodeGenerationRootNamespace}.Models.Abstractions"
        //            && !SkipNamespaceOnTypenameMappings(x.Namespace)
        //            && x.FullName is not null)
        //        .SelectMany(x =>
        //            new[]
        //            {
        //                new TypenameMappingBuilder().WithSourceTypeName(x.FullName!).WithTargetTypeName($"{ProjectName}.{x.Name}"),
        //                new TypenameMappingBuilder().WithSourceTypeName($"{ProjectName}.{x.Name.Substring(1)}").WithTargetTypeName($"{ProjectName}.{x.Name}"), // hacking
        //                new TypenameMappingBuilder().WithSourceTypeName($"{ProjectName}.{x.Name}").WithTargetTypeName($"{ProjectName}.{x.Name}")
        //                    .AddMetadata
        //                    (
        //                        new MetadataBuilder().WithValue($"{ProjectName}.Builders").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
        //                        new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
        //                        new MetadataBuilder().WithValue($"{ProjectName}.Builders").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
        //                        new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceName),
        //                        new MetadataBuilder().WithValue("[Name][NullableSuffix].ToBuilder()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression),
        //                        new MetadataBuilder().WithValue("[Name][NullableSuffix].Build()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression),
        //                        new MetadataBuilder().WithName(ClassFramework.Pipelines.MetadataNames.CustomEntityInterfaceTypeName).WithValue($"{ProjectName}.I{x.GetEntityClassName()}")
        //                    )
        //            });

        //foreach (var type in abstractionTypes)
        //{
        //    yield return type;
        //}
    }

    // Skip builder pattern on abstractions (Most importantly, IOperator, because we generate them manually. But also on IParseResult, which is only used for removing code duplication on parse results)
    // protected override bool UseBuilderAbstractionsTypeConversion => false;
}
