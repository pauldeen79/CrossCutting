using System.CodeDom.Compiler;
using System.Linq;
using CrossCutting.Utilities.QueryEvaluator.Core;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;

namespace PDC.Net.Core.Queries
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.QueryBuilderGenerator", @"1.0.0.0")]
    public class ExtraFieldQueryBuilder : QueryBaseBuilder<ExtraFieldQueryBuilder, ExtraFieldQuery>
    {
        public ExtraFieldQueryBuilder() : base()
        {
        }

        public ExtraFieldQueryBuilder(QueryBase source) : base(source)
        {
        }

        public override ExtraFieldQuery BuildTyped()
        {
            return new ExtraFieldQuery(Limit, Offset, Conditions?.Select(x => x.Build()), SortOrders?.Select(x => x.Build()));
        }

        public static implicit operator ExtraFieldQuery(ExtraFieldQueryBuilder builder)
        {
            return builder.BuildTyped();
        }
    }
}
