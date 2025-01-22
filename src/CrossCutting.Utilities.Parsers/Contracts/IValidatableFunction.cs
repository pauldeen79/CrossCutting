namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IValidatableFunction : IFunction
{
    Result<Type> Validate(FunctionCallContext context);
}
