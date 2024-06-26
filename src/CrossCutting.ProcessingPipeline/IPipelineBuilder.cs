﻿namespace CrossCutting.ProcessingPipeline;

public interface IPipelineBuilder<TRequest>
{
    IList<IBuilder<IPipelineComponent<TRequest>>> Components { get; }

    public IPipeline<TRequest> Build();
}

public interface IPipelineBuilder<TRequest, TResponse>
{
    IList<IBuilder<IPipelineComponent<TRequest, TResponse>>> Components { get; }

    public IPipeline<TRequest, TResponse> Build();
}
