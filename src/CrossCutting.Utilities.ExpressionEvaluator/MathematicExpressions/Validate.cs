namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions;

public partial class Validate(IEnumerable<IMathematicExpressionValidator> validators) : IMathematicExpression
{
    private readonly IEnumerable<IMathematicExpressionValidator> _validators = validators;

    public Result<MathematicExpressionState> Evaluate(MathematicExpressionState state)
        => _validators.Pipe
        (
            Result.FromInstance(state),
            (current, validator) => validator.Validate(current.Value!),
            result => result.IsSuccessful()
        );
}
