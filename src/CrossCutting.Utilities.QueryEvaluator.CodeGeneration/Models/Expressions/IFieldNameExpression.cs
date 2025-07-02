namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IFieldNameExpression : IExpressionBase
{
    [Required] string FieldName { get; }
}
