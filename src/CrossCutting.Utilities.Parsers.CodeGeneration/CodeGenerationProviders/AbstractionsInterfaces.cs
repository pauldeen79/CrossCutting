namespace CrossCutting.Utilities.Parsers.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(ICommandService commandService) : CrossCuttingCSharpClassBase(commandService)
{
    public override string Path => "CrossCutting.Utilities.Parsers/Abstractions";

    protected override bool EnableEntityInheritance => true;

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken token)
        => GetEntityInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.Parsers", "CrossCutting.Utilities.Parsers.Abstractions");
}
