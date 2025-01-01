namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersExtensions(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken) => GetBuilderExtensions(GetAbstractionsInterfaces(), "CrossCutting.Utilities.Parsers.Builders.Abstractions", "CrossCutting.Utilities.Parsers.Abstractions", "CrossCutting.Utilities.Parsers.Builders.Extensions");

    public override string Path => "CrossCutting.Utilities.Parsers/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}
