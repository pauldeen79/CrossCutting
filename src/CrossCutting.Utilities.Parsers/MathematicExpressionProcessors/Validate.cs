namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal partial class Validate : IMathematicExpressionProcessor
{
    private static readonly IMathematicExpressionValidator[] _validators = new IMathematicExpressionValidator[]
    {
        new NullOrEmptyValidator(),
        new TemporaryDelimiterValidator(),
        new StartWithOperatorValidator(),
        new EndWithOperatorValidator(),
        new EmptyValuePartValidator(),
        new BracketValidator(),
    };

    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
        => Result<MathematicExpressionState>.Pipe
        (
            state,
            _validators,
            (result, validator) => validator.Validate(result.Value!)
        );
}
