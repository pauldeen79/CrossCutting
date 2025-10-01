using CrossCutting.Common.Abstractions;

namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces(IPipelineService pipelineService) : QueryEvaluatorCSharpClassBase(pipelineService)
{
    public override async Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
    {
        // Work-around to fix 'new' keyword on ToBuilder method, which is not detected correctly because I'm adding the intheritance after code generation.
        // For some reason, it doesn't work to add the interface to code generation
        var result = await GetEntityInterfacesAsync(GetAbstractionsInterfacesAsync(), Constants.Namespaces.UtilitiesQueryEvaluator, CurrentNamespace).ConfigureAwait(false);

        if (!result.IsSuccessful())
        {
            return result;
        }

        var items = new List<TypeBase>();

        foreach (var type in result.Value!)
        {
            if (type.Name != nameof(ICondition))
            {
                items.Add(type);
                continue;
            }

            items.Add(type
                .ToBuilder()
                .With(x => x.Methods.Single(x => x.Name == nameof(IBuildableEntity<ICondition>.ToBuilder)).WithNew())
                .Build());
        }

        return Result.Success(items.AsEnumerable());
    }

    public override string Path => $"{Constants.Namespaces.UtilitiesQueryEvaluator}.Abstractions";

    protected override bool EnableEntityInheritance => true;
}
