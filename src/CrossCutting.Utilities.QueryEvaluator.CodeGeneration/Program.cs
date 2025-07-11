using ClassFramework.Domain.Extensions;
using CrossCutting.Common.Extensions;

namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static async Task Main(string[] args)
    {
        // Setup code generation
        var currentDirectory = Directory.GetCurrentDirectory();
        var basePath = currentDirectory switch
        {
            var x when x.EndsWith(Constants.ProjectName) => Path.Combine(currentDirectory, @"src/"),
            var x when x.EndsWith($"{Constants.ProjectName}.CodeGeneration") => Path.Combine(currentDirectory, @"../"),
            _ => Path.Combine(currentDirectory, @"../../../../")
        };
        var services = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddClassFrameworkPipelines()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkRuntime()
            .AddCsharpExpressionDumper()
            .AddClassFrameworkTemplates()
            .AddScoped<IAssemblyInfoContextService, MyAssemblyInfoContextService>();

        var generators = typeof(Program).Assembly.GetExportedTypes()
            .Where(x => !x.IsAbstract && x.BaseType == typeof(QueryEvaluatorCSharpClassBase))
            .ToArray();

        foreach (var type in generators)
        {
            services.AddScoped(type);
        }

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var processed = false;

        // Generate code
        foreach (var generatorType in generators)
        {
            var generator = (QueryEvaluatorCSharpClassBase)scope.ServiceProvider.GetRequiredService(generatorType);

            if (!processed)
            {
                processed = true;
                foreach (var mapping in generator.GetTypenameMappings().Where(m => m.SourceTypeName.GetClassName().In(nameof(ICondition), nameof(ICondition).Substring(1), nameof(ISingleExpressionContainer), nameof(ISingleExpressionContainer).Substring(1))))
                {
                    Console.WriteLine($"{mapping.SourceTypeName} -> {mapping.TargetTypeName}");
                    foreach (var md in mapping.Metadata)
                    {
                        Console.WriteLine($"    {md.Name}: {md.Value}");
                    }
                }
                foreach (var mapping in generator.GetNamespaceMappings())
                {
                    Console.WriteLine($"{mapping.SourceNamespace} -> {mapping.TargetNamespace}");
                    foreach (var md in mapping.Metadata)
                    {
                        Console.WriteLine($"    {md.Name}: {md.Value}");
                    }
                }
            }

            var result = await engine.GenerateAsync(generator, new MultipleStringContentBuilderEnvironment(), new CodeGenerationSettings(basePath, Path.Combine(generator.Path, $"{generatorType.Name}.template.generated.cs"))).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                Console.WriteLine("Errors:");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                WriteError(result);
                break;
            }
        }

        // Log output to console
        if (!string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine($"Code generation completed, check the output in {basePath}");
        }
    }

    private static void WriteError(Result error)
    {
        Console.WriteLine($"{error.Status} {error.ErrorMessage}");
        foreach (var validationError in error.ValidationErrors)
        {
            Console.WriteLine($"{string.Join(",", validationError.MemberNames)}: {validationError.ErrorMessage}");
        }

        foreach (var innerResult in error.InnerResults)
        {
            WriteError(innerResult);
        }
    }
}
