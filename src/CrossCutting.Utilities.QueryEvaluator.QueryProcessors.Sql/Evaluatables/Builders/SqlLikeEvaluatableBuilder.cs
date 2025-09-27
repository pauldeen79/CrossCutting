namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Evaluatables.Builders;

public partial class SqlLikeEvaluatableBuilder : IEvaluatableBuilder
{
    IEvaluatable IBuilder<IEvaluatable>.Build()
        => new SqlLikeEvaluatable(SourceExpression?.Build()!, FormatString);
}
