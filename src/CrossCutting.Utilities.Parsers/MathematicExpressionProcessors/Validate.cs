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
        => _validators.Aggregate
        (
            Result<MathematicExpressionState>.Success(state),
            (result, validator) => result.IsSuccessful()
                ? validator.Validate(result.Value!)
                : result
        );
}
