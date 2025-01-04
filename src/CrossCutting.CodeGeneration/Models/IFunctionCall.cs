namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionCall
{
    [Required] string Name { get; }
    //[Description("Optional Id to use in case of function overload resolution")] [Required(AllowEmptyStrings = true)] string Id { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionCallArgument> Arguments { get; }
}
