namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IFieldNameExpression : IExpression
{
    [Required] string FieldName { get; }
}
