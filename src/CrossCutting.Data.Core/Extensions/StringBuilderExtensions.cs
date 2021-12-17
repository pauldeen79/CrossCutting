using System.Collections.Generic;
using System.Text;

namespace CrossCutting.Data.Core.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static StringBuilder AppendPagingOuterQuery(this StringBuilder builder,
                                                             StringBuilder selectBuilder,
                                                             int? offset,
                                                             bool countOnly)
            => offset.HasValue && offset.Value > 0 && !countOnly
                ? builder
                    .Append("SELECT ")
                    .AppendSelectFields(selectBuilder, countOnly)
                    .AppendFromClause()
                    .Append("(")
                : builder;

        internal static StringBuilder AppendFromClause(this StringBuilder builder)
            => builder.Append(" FROM ");

        internal static StringBuilder AppendSelectFields(this StringBuilder builder,
                                                         StringBuilder selectBuilder,
                                                         bool countOnly)
        {
            if (countOnly)
            {
                return builder.Append("COUNT(*)");
            }

            if (selectBuilder.Length == 0)
            {
                return builder.Append("*");
            }

            return builder.Append(selectBuilder);
        }

        internal static StringBuilder AppendSelectAndDistinctClause(this StringBuilder builder,
                                                                    bool distinct,
                                                                    bool countOnly)
        {
            builder.Append("SELECT ");
            if (distinct && !countOnly)
            {
                builder.Append("DISTINCT ");
            }

            return builder;
        }

        internal static StringBuilder AppendTopClause(this StringBuilder builder,
                                                      StringBuilder orderByBuilder,
                                                      int? offset,
                                                      int? pageSize,
                                                      bool countOnly)
        {
            if ((offset == null || offset.Value <= 0 || orderByBuilder.Length == 0)
                && pageSize.HasValue 
                && pageSize.Value > 0
                && !countOnly)
            {
                return builder.Append($"TOP {pageSize} ");
            }

            return builder;
        }

        internal static StringBuilder AppendCountOrSelectFields(this StringBuilder builder,
                                                                StringBuilder selectBuilder,
                                                                bool countOnly)
            => countOnly
                ? builder.Append("COUNT(*)")
                : builder.AppendSelectFields(selectBuilder, countOnly);

        internal static StringBuilder AppendPagingPrefix(this StringBuilder instance,
                                                         StringBuilder orderByBuilder,
                                                         int? offset,
                                                         bool countOnly)
        {
            if (offset.HasValue && offset.Value > 0 && !countOnly)
            {
                if (orderByBuilder.Length == 0)
                {
                    orderByBuilder.Append("(SELECT 0)");
                }

                return instance.Append($", ROW_NUMBER() OVER (ORDER BY {orderByBuilder}) as sq_row_number");
            }

            return instance;
        }

        internal static StringBuilder AppendTableName(this StringBuilder builder,
                                                      StringBuilder fromBuilder)
            => builder.Append(fromBuilder);

        internal static StringBuilder AppendWhereClause(this StringBuilder instance,
                                                        StringBuilder whereBuilder,
                                                        int? offset,
                                                        int? pageSize)
        {
            if (whereBuilder.Length == 0)
            {
                return instance;
            }

            return instance.Append(" WHERE ").Append(whereBuilder);
        }

        internal static StringBuilder AppendGroupByClause(this StringBuilder instance,
                                                          StringBuilder groupByBuilder)
        {
            if (groupByBuilder.Length == 0)
            {
                return instance;
            }

            return instance.Append(" GROUP BY ").Append(groupByBuilder);
        }

        internal static StringBuilder AppendHavingClause(this StringBuilder instance,
                                                         StringBuilder havingBuilder)
        {
            if (havingBuilder.Length == 0)
            {
                return instance;
            }

            return instance.Append(" HAVING ").Append(havingBuilder);
        }

        internal static StringBuilder AppendOrderByClause(this StringBuilder instance,
                                                          StringBuilder orderByBuilder,
                                                          int? offset,
                                                          bool countOnly)
        {
            if (offset.HasValue && offset.Value > 0)
            {
                //do not use order by (this will be taken care of by the row_number function)
                return instance;
            }
            else if (countOnly)
            {
                //do not use order by, only count the number of records
                return instance;
            }
            else if (orderByBuilder.Length > 0)
            {
                return instance.Append(" ORDER BY ").Append(orderByBuilder);
            }
            else
            {
                return instance;
            }
        }

        internal static StringBuilder AppendPagingSuffix(this StringBuilder instance,
                                                         int? offset,      
                                                         int? pageSize,
                                                         bool countOnly)
        {
            if (offset.HasValue && offset.Value > 0 && !countOnly)
            {
                if (pageSize != null && pageSize > 0)
                {
                    return instance.Append($") sq WHERE sq.sq_row_number BETWEEN {offset.Value + 1} and {offset.Value + pageSize.Value};");
                }
                else
                {
                    return instance.Append($") sq WHERE sq.sq_row_number > {offset.Value};");
                }
            }

            return instance;
        }

        internal static StringBuilder AppendInsert(this StringBuilder instance,
                                                   string table,
                                                   string temporaryTable,
                                                   ICollection<string> fieldNames,
                                                   ICollection<string> outputFields)
            => instance
               .Append("INSERT INTO ")
               .Append(table)
               .Append("(")
               .Append(string.Join(", ", fieldNames))
               .Append(")")
               .AppendOutputFields(temporaryTable, outputFields);

        internal static StringBuilder AppendOutputFields(this StringBuilder instance, string temporaryTable, ICollection<string> outputFields)
        {
            if (outputFields.Count > 0)
            {
                instance.Append(" OUTPUT ")
                        .Append(string.Join(", ", outputFields));
            }

            if (!string.IsNullOrEmpty(temporaryTable))
            {
                instance.Append(" INTO ")
                        .Append(temporaryTable);
            }

            return instance;
        }
    }
}
