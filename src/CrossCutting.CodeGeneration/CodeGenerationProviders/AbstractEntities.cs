﻿namespace CrossCutting.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractEntities(IPipelineService pipelineService) : CrossCuttingCSharpClassBase(pipelineService)
{
    public override string Path => Constants.Namespaces.UtilitiesParsers;

    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.None; // not needed for abstract entities, because each derived class will do its own validation

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetAbstractModelsAsync(), CurrentNamespace);
}
