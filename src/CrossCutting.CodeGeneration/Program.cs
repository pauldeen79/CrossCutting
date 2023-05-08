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
        var generateMultipleFiles = true;
        var dryRun = false;
        var multipleContentBuilder = new MultipleContentBuilder { BasePath = basePath };
        var settings = new CodeGenerationSettings(basePath, generateMultipleFiles, dryRun);

        // Generate code
        var expressionFrameworkGenerators = typeof(CrossCuttingCSharpClassBase).Assembly.GetExportedTypes().Where(x => x.BaseType == typeof(CrossCuttingCSharpClassBase) && !x.IsAbstract).ToArray();
        _ = expressionFrameworkGenerators.Select(x => (CrossCuttingCSharpClassBase)Activator.CreateInstance(x)!).Select(x => GenerateCode.For(x.GetSettings(settings), multipleContentBuilder, x)).ToArray();

        // Log output to console
        if (string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine(multipleContentBuilder.ToString());
        }
        else
        {
            Console.WriteLine($"Code generation completed, check the output in {basePath}");
            Console.WriteLine($"Generated files: {multipleContentBuilder.Contents.Count()}");
        }
    }
}
