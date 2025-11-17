namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces(ICommandService commandService) : ExpressionEvaluatorCSharpClassBase(commandService)
{
    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
    {
        return GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Abstractions", "CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions");
    }

    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders/Abstractions";
    
    protected override bool EnableEntityInheritance => true;
}
