﻿namespace CrossCutting.ProcessingPipeline;

public interface IPipelineFeature<TModel>
{
    void Process(PipelineContext<TModel> context);
}

public interface IPipelineFeature<TModel, TContext>
{
    void Process(PipelineContext<TModel, TContext> context);
}