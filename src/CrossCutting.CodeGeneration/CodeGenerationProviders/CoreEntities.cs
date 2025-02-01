﻿namespace CrossCutting.CodeGeneration.CodeGenerationProviders.FunctionParseResultArguments;

[ExcludeFromCodeCoverage]
public class CoreEntities(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    protected override bool UseBuilderAbstractionsTypeConversion => false; //quirk

    public override string Path => Constants.Namespaces.UtilitiesParsers;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetEntities(GetCoreModels(), CurrentNamespace);
}
