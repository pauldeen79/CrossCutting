namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public partial class Validate : IMathematicExpressionProcessor
{
    private readonly IEnumerable<IMathematicExpressionValidator> _validators;

    public Validate(IEnumerable<IMathematicExpressionValidator> validators)
    {
        _validators = validators;
    }

    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
        => Result<MathematicExpressionState>.Pipe
        (
            state,
            _validators,
            (result, validator) => validator.Validate(result.Value!)
        );
}
