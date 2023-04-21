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
    {
        foreach (var validator in _validators)
        {
            var result = validator.Validate(state);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
