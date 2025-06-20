﻿namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders(IPipelineService pipelineService) : ExpressionEvaluatorCSharpClassBase(pipelineService)
{
    public override string Path => $"{Constants.Namespaces.UtilitiesExpressionEvaluator}/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuildersAsync(GetCoreModelsAsync(), CurrentNamespace, Constants.Namespaces.UtilitiesExpressionEvaluator);

    protected override bool CreateAsObservable => true;
}
