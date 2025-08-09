namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration;

[ExcludeFromCodeCoverage]
public class MyAssemblyInfoContextService : IAssemblyInfoContextService
{
    public string[] GetExcludedAssemblies() =>
    [
        "System.Runtime",
        "System.Collections",
        "System.ComponentModel",
        "TemplateFramework.Abstractions",
        "TemplateFramework.Core",
        "TemplateFramework.Core.CodeGeneration",
        "TemplateFramework.Runtime",
        "TemplateFramework.TemplateProviders.ChildTemplateProvider",
        "CrossCutting.Common",
        "CrossCutting.ProcessingPipeline",
        "CrossCutting.Utilities.Aggregators",
        "CrossCutting.Utilities.QueryEvaluator",
        "CrossCutting.Utilities.Operators",
        "Microsoft.Extensions.DependencyInjection",
        "Microsoft.Extensions.DependencyInjection.Abstractions",
        "ClassFramework.CsharpExpressionCreator",
        "ClassFramework.Domain",
        "ClassFramework.Pipelines",
        "ClassFramework.TemplateFramework",
        "CsharpExpressionDumper.Abstractions",
        "CsharpExpressionDumper.Core",
        "CrossCutting.Utilities.QueryEvaluator.CodeGeneration",
    ];
}
