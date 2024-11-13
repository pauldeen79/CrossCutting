namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public partial class Validate(IEnumerable<IMathematicExpressionValidator> validators) : IMathematicExpressionProcessor
{
    private readonly IEnumerable<IMathematicExpressionValidator> _validators = validators;

    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
        => _validators.Pipe
        (
            Result.FromInstance(state),
            (current, validator) => validator.Validate(current.Value!),
            result => result.IsSuccessful()
        );
}
