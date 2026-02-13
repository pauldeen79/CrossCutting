using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Core;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;
using CrossCutting.Utilities.QueryEvaluator.Core.Evaluatables;

namespace PDC.Net.Core.Queries
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.QueryGenerator", @"1.0.0.0")]
    public partial record CatalogQuery : QueryBase, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Limit.HasValue && Limit.Value > MaxLimit)
            {
                yield return new ValidationResult("Limit exceeds the maximum of " + MaxLimit, new[] { nameof(Limit), nameof(Limit) });
            }

            foreach (var condition in Conditions)
            {
                if (!IsValidExpression(condition))
                {
                    yield return new ValidationResult("Invalid field name in condition: " + condition, new[] { nameof(Conditions) });
                }
            }
            foreach (var querySortOrder in SortOrders)
            {
                if (!IsValidExpression(querySortOrder.Expression))
                {
                    yield return new ValidationResult("Invalid field name in order by expression: " + querySortOrder.Expression, new[] { nameof(SortOrders) });
                }
            }
        }

        private bool IsValidExpression(IEvaluatable expression)
        {
            //TODO: Fix nested expressions. You might get an EqualCondition with a left or right expression of type PropertyNameEvaluatable...
            if (expression is PropertyNameEvaluatable fieldExpression)
            {
                // default: var result = false;
                // Override because of extrafields transformation
                var result = true;

                // Expression can't be validated here because of support of dynamic extrafields
                //if (expression is PdcCustomQueryExpression) return true;

                return result || ValidFieldNames.Any(s => s.Equals(fieldExpression.PropertyName, StringComparison.OrdinalIgnoreCase));
            }

            // You might want to validate the expression to prevent sql injection (unless you can only create query expressions in code)
            return expression.GetType().Assembly.FullName.StartsWith("CrossCutting.Utilities.ExpressionEvaluator");
        }

        public override QueryBaseBuilder ToBuilder()
        {
            return new CatalogQueryBuilder(this);
        }

        public CatalogQuery() : this(null, null, Enumerable.Empty<ICondition>(), Enumerable.Empty<ISortOrder>())
        {
        }

        public CatalogQuery(int? limit,
                            int? offset,
                            IEnumerable<ICondition> conditions,
                            IEnumerable<ISortOrder> orderByFields)
            : base(limit, offset, conditions, orderByFields)
        {
        }

        public CatalogQuery(IQuery query): this(query.Limit,
                                                query.Offset,
                                                query.Conditions,
                                                query.SortOrders)
        {
        }

        private static readonly string[] ValidFieldNames = new[] { "Id", "Name", "DateCreated", "DateLastModified", "DateSynchronized", "DriveSerialNumber", "DriveTypeCodeType", "DriveTypeCode", "DriveTypeDescription", "DriveTotalSize", "DriveFreeSpace", "Recursive", "Sorted", "StartDirectory", "ExtraField1", "ExtraField2", "ExtraField3", "ExtraField4", "ExtraField5", "ExtraField6", "ExtraField7", "ExtraField8", "ExtraField9", "ExtraField10", "ExtraField11", "ExtraField12", "ExtraField13", "ExtraField14", "ExtraField15", "ExtraField16", "IsExistingEntity", "AllFields" };

        private const int MaxLimit = int.MaxValue;
    }
}

