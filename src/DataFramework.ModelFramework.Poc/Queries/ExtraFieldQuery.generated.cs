using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Core;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;
using CrossCutting.Utilities.QueryEvaluator.Core.Evaluatables;
using DataFramework.ModelFramework.Poc.Extensions;

namespace PDC.Net.Core.Queries
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.QueryGenerator", @"1.0.0.0")]
    public partial record ExtraFieldQuery : QueryBase, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Limit.HasValue && Limit.Value > MaxLimit)
            {
                yield return new ValidationResult("Limit exceeds the maximum of " + MaxLimit, new[] { nameof(Limit) });
            }

            foreach (var condition in Conditions)
            {
                if (!condition.IsValidExpression(ValidFieldNames))
                {
                    yield return new ValidationResult("Invalid field name in condition: " + condition, new[] { nameof(Conditions) });
                }
            }
            foreach (var querySortOrder in SortOrders)
            {
                if (!querySortOrder.Expression.IsValidExpression(ValidFieldNames))
                {
                    yield return new ValidationResult("Invalid field name in order by expression: " + querySortOrder.Expression, new[] { nameof(SortOrders) });
                }
            }
        }

        public override QueryBaseBuilder ToBuilder()
        {
            return new ExtraFieldQueryBuilder(this);
        }

        public ExtraFieldQuery() : this(null, null, Enumerable.Empty<ICondition>(), Enumerable.Empty<ISortOrder>())
        {
        }

        public ExtraFieldQuery(int? limit,
                               int? offset,
                               IEnumerable<ICondition> conditions,
                               IEnumerable<ISortOrder> orderByFields)
            : base(limit, offset, conditions, orderByFields)
        {
        }

        public ExtraFieldQuery(IQuery query): this(query.Limit,
                                                   query.Offset,
                                                   query.Conditions,
                                                   query.SortOrders)
        {
        }

        private static readonly string[] ValidFieldNames = new[] { "EntityName", "Description", "FieldNumber", "FieldType" };

        private const int MaxLimit = int.MaxValue;
    }
}

