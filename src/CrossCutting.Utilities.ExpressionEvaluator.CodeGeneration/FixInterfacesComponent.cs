namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration;

public class FixInterfacesComponent : IPipelineComponent<GenerateBuilderCommand, ClassBuilder>
{
    public Task<Result> ExecuteAsync(GenerateBuilderCommand commmand, ClassBuilder response, ICommandService commandService, CancellationToken token)
        => Task.Run(() =>
        {
            var interfaces = response.Interfaces.Distinct().ToArray();
            response.Interfaces.Clear();
            response.Interfaces.AddRange(interfaces);
            return Result.Success();
        }, token);
}
