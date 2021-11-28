using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Core.Extensions;

namespace CrossCutting.Data.Sql.Builders
{
    public class PagedSelectCommandBuilder
    {
        public IDictionary<string, object> CommandParameters { get; set; }
        private StringBuilder SelectBuilder { get; }
        private StringBuilder FromBuilder { get; }
        private StringBuilder WhereBuilder { get; }
        private StringBuilder OrderByBuilder { get; }
        private StringBuilder GroupByBuilder { get; }
        private StringBuilder HavingBuilder { get; }
        public bool Distinct { get; set; }
        public int? Offset { get; set; }
        public int? PageSize { get; set; }

        public PagedSelectCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            SelectBuilder = new StringBuilder();
            FromBuilder = new StringBuilder();
            WhereBuilder = new StringBuilder();
            OrderByBuilder = new StringBuilder();
            GroupByBuilder = new StringBuilder();
            HavingBuilder = new StringBuilder();
        }

        public PagedSelectCommandBuilder DistinctValues(bool distinct = true)
            => this.Chain(() => Distinct = distinct);

        public PagedSelectCommandBuilder WithTop(int? top)
            => this.Chain(() =>
            {
                PageSize = top;
                Offset = null;
            });

        public PagedSelectCommandBuilder Select(string value)
            => this.Chain(() => SelectBuilder.Append(value));

        public PagedSelectCommandBuilder From(string value)
            => this.Chain(() => FromBuilder.Append(value));

        public PagedSelectCommandBuilder InnerJoin(string value)
            => this.Chain(() =>
            {
                if (FromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
                }
                FromBuilder.Append(" INNER JOIN ").Append(value);
            });

        public PagedSelectCommandBuilder Skip(int? offset)
            => this.Chain(() => Offset = offset);

        public PagedSelectCommandBuilder Take(int? pageSize)
            => this.Chain(() => PageSize = pageSize);

        public PagedSelectCommandBuilder LeftOuterJoin(string value)
            => this.Chain(() =>
            {
                if (FromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
                }
                FromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            });

        public PagedSelectCommandBuilder RightOuterJoin(string value)
            => this.Chain(() =>
            {
                if (FromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
                }
                FromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            });

        public PagedSelectCommandBuilder CrossJoin(string value)
            => this.Chain(() =>
            {
                if (FromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
                }
                FromBuilder.Append(" CROSS JOIN ").Append(value);
            });

        public PagedSelectCommandBuilder Where(string value)
            => this.Chain(() =>
            {
                if (WhereBuilder.Length > 0)
                {
                    WhereBuilder.Append(" AND ");
                }
                WhereBuilder.Append(value);
            });

        public PagedSelectCommandBuilder And(string value)
            => Where(value);

        public PagedSelectCommandBuilder Or(string value)
            => this.Chain(() =>
            {
                if (WhereBuilder.Length == 0)
                {
                    throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
                }
                WhereBuilder.Append(" OR ").Append(value);
            });

        public PagedSelectCommandBuilder OrderBy(string value)
            => this.Chain(() => OrderByBuilder.Append(value));

        public PagedSelectCommandBuilder GroupBy(string value)
            => this.Chain(() => GroupByBuilder.Append(value));

        public PagedSelectCommandBuilder Having(string value)
            => this.Chain(() => HavingBuilder.Append(value));

        public PagedSelectCommandBuilder AppendParameter(string key, object value)
            => this.Chain(() => CommandParameters.Add(key, value));

        public PagedSelectCommandBuilder AppendParameters(object parameters)
            => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

        public PagedSelectCommandBuilder Clear()
        {
            SelectBuilder.Clear();
            FromBuilder.Clear();
            WhereBuilder.Clear();
            OrderByBuilder.Clear();
            GroupByBuilder.Clear();
            HavingBuilder.Clear();
            CommandParameters.Clear();
            Distinct = false;
            Offset = 0;
            PageSize = 0;
            return this;
        }

        public IPagedDatabaseCommand Build()
            => new PagedDatabaseCommand(CreateCommand(false),
                                        CreateCommand(true),
                                        Offset.GetValueOrDefault(),
                                        PageSize.GetValueOrDefault(int.MaxValue));

        private IDatabaseCommand CreateCommand(bool countOnly)
            => new SqlDatabaseCommand(BuildSql(countOnly), DatabaseCommandType.Text, DatabaseOperation.Select, CommandParameters);

        private string BuildSql(bool countOnly)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("FROM clause is missing");
            }

            return new StringBuilder().AppendPagingOuterQuery(SelectBuilder, Offset, countOnly)
                .AppendSelectAndDistinctClause(Distinct, countOnly)
                .AppendTopClause(OrderByBuilder, Offset, PageSize, countOnly)
                .AppendCountOrSelectFields(SelectBuilder, countOnly)
                .AppendPagingPrefix(OrderByBuilder, Offset, countOnly)
                .AppendFromClause()
                .AppendTableName(FromBuilder)
                .AppendWhereClause(WhereBuilder, Offset, PageSize)
                .AppendGroupByClause(GroupByBuilder)
                .AppendHavingClause(HavingBuilder)
                .AppendOrderByClause(OrderByBuilder, Offset, countOnly)
                .AppendPagingSuffix(Offset, PageSize, countOnly)
                .ToString();
        }
    }
}
