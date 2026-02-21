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

        //HACK: Add support for using builder abstraction type conversion on IEvaluatable
        var x = typeof(IEvaluatable);
        yield return new TypenameMappingBuilder("CrossCutting.Utilities.ExpressionEvaluator.Abstractions.IEvaluatable")
            .AddMetadata
            (
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace, $"{CoreNamespace}.Builders{x.Namespace.ReplaceStartNamespace($"{CodeGenerationRootNamespace}.Models", false)}"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderName, $"I{x.GetEntityClassName()}Builder"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceNamespace, /*$"{AbstractionsNamespace}.Builders"*/ "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceName, $"I{x.GetEntityClassName()}Builder"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceTypeName, $"{AbstractionsNamespace}.Builders.I{x.GetEntityClassName()}Builder"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression, /*x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions" && Array.Exists(x.GetInterfaces(), IsAbstractType)
                    ? $"new {CoreNamespace}.Builders{x.Namespace.ReplaceStartNamespace($"{CodeGenerationRootNamespace}.Models", false)}.{x.GetEntityClassName()}Builder([Name])"
                    : */"[Name][NullableSuffix].ToBuilder()[ForcedNullableSuffix]"),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue, /*x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions" && IsAbstractType(x)
                    ? */new Literal($"default({CoreNamespace}.Builders{x.Namespace.ReplaceStartNamespace($"{CodeGenerationRootNamespace}.Models", false)}.I{x.GetEntityClassName()}Builder)")
                    /*: new Literal($"new {CoreNamespace}.Builders{x.Namespace.ReplaceStartNamespace($"{CodeGenerationRootNamespace}.Models", false)}.{x.GetEntityClassName()}Builder()")*/),
                new MetadataBuilder(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression, x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions" && Array.Exists(x.GetInterfaces(), IsAbstractType)
                    ? "[Name][NullableSuffix].BuildTyped()[ForcedNullableSuffix]"
                    : "[Name][NullableSuffix].Build()[ForcedNullableSuffix]")
            );
    }

    // Skip builder pattern on abstractions (Most importantly, IOperator, because we generate them manually. But also on IParseResult, which is only used for removing code duplication on parse results)
    protected override bool UseBuilderAbstractionsTypeConversion => false;
}
