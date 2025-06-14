namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => "CrossCutting.Utilities.Parsers/Builders/Abstractions";

    protected override bool EnableEntityInheritance => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.Parsers.Builders.Abstractions", "CrossCutting.Utilities.Parsers.Abstractions", "CrossCutting.Utilities.Parsers.Builders.Abstractions");
}
