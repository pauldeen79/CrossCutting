using System.CodeDom.Compiler;
using System.Linq;
using CrossCutting.Utilities.QueryEvaluator.Core;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;

namespace PDC.Net.Core.Queries
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.QueryBuilderGenerator", @"1.0.0.0")]
    public class CatalogQueryBuilder : QueryBaseBuilder<CatalogQueryBuilder, CatalogQuery>
    {
        public CatalogQueryBuilder() : base()
        {
        }

        public CatalogQueryBuilder(QueryBase source) : base(source)
        {
        }

        public override CatalogQuery BuildTyped()
        {
            return new CatalogQuery(Limit, Offset, Conditions?.Select(x => x.Build()), SortOrders?.Select(x => x.Build()));
        }

        public static implicit operator CatalogQuery(CatalogQueryBuilder builder)
        {
            return builder.BuildTyped();
        }
    }
}
