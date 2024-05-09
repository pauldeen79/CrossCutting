namespace CrossCutting.CodeGeneration;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {
        // Setup code generation
        var currentDirectory = Directory.GetCurrentDirectory();
        var basePath = currentDirectory switch
        {
            var x when x.EndsWith(Constants.ProjectName) => Path.Combine(currentDirectory, @"src/"),
            var x when x.EndsWith(Constants.Namespaces.UtilitiesParsers) => Path.Combine(currentDirectory, @"../"),
            var x when x.EndsWith($"{Constants.ProjectName}.CodeGeneration") => Path.Combine(currentDirectory, @"../"),
            _ => Path.Combine(currentDirectory, @"../../../../")
        };
        var services = new ServiceCollection()
            .AddParsers()
            .AddPipelines()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkRuntime()
            .AddCsharpExpressionDumper()
            .AddClassFrameworkTemplates()
            .AddScoped<IAssemblyInfoContextService, MyAssemblyInfoContextService>();

        var generators = typeof(Program).Assembly.GetExportedTypes()
            .Where(x => !x.IsAbstract && x.BaseType == typeof(CrossCuttingCSharpClassBase))
            .ToArray();

        foreach (var type in generators)
        {
            services.AddScoped(type);
        }

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var instances = generators
            .Select(x => (ICodeGenerationProvider)scope.ServiceProvider.GetRequiredService(x))
            .ToArray();
        var engine = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();

        // Generate code
        var tasks = instances.Select(x => engine.Generate(x, new MultipleContentBuilderEnvironment(), new CodeGenerationSettings(basePath, Path.Combine(x.Path, $"{x.GetType().Name}.template.generated.cs")))).ToArray();
        Task.WaitAll(tasks);

        // Log output to console
        if (!string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine($"Code generation completed, check the output in {basePath}");
        }
    }
}
