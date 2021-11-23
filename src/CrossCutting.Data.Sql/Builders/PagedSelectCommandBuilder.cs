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
        public DatabaseCommandType CommandType { get; set; }
        public IDictionary<string, object> CommandParameters { get; set; }
        private StringBuilder SelectBuilder { get; }
        private StringBuilder FromBuilder { get; }
        private StringBuilder WhereBuilder { get; }
        private StringBuilder OrderByBuilder { get; }
        private StringBuilder GroupByBuilder { get; }
        private StringBuilder HavingBuilder { get; }
        private bool _distinct;
        private int? _offset;
        private int? _pageSize;

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

        public PagedSelectCommandBuilder Distinct(bool distinct = true)
        {
            _distinct = distinct;
            return this;
        }

        public PagedSelectCommandBuilder Top(int top)
        {
            _pageSize = top;
            _offset = null;
            return this;
        }

        public PagedSelectCommandBuilder Select(string value)
        {
            SelectBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder From(string value)
        {
            FromBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder InnerJoin(string value)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
            }
            FromBuilder.Append(" INNER JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder Offset(int? offset)
        {
            _offset = offset;
            return this;
        }

        public PagedSelectCommandBuilder PageSize(int? pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public PagedSelectCommandBuilder LeftOuterJoin(string value)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
            }
            FromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder RightOuterJoin(string value)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
            }
            FromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder CrossJoin(string value)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
            }
            FromBuilder.Append(" CROSS JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder Where(string value)
        {
            if (WhereBuilder.Length > 0)
            {
                WhereBuilder.Append(" AND ");
            }
            WhereBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder And(string value)
            => Where(value);

        public PagedSelectCommandBuilder Or(string value)
        {
            if (WhereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            WhereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder OrderBy(string value)
        {
            OrderByBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder GroupBy(string value)
        {
            GroupByBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder Having(string value)
        {
            HavingBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public PagedSelectCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public PagedSelectCommandBuilder Clear()
        {
            SelectBuilder.Clear();
            FromBuilder.Clear();
            WhereBuilder.Clear();
            OrderByBuilder.Clear();
            GroupByBuilder.Clear();
            HavingBuilder.Clear();
            CommandParameters.Clear();
            _distinct = false;
            _offset = null;
            _pageSize = null;
            return this;
        }

        public PagedSelectCommandBuilder AsStoredProcedure()
        {
            CommandType = DatabaseCommandType.StoredProcedure;
            return this;
        }

        public PagedSelectCommandBuilder AsText()
        {
            CommandType = DatabaseCommandType.Text;
            return this;
        }

        public IDatabaseCommand Build(bool countOnly)
            => new SqlDatabaseCommand(BuildSql(countOnly), CommandType, DatabaseOperation.Select, CommandParameters);

        private string BuildSql(bool countOnly)
        {
            if (FromBuilder.Length == 0)
            {
                throw new InvalidOperationException("FROM clause is missing");
            }

            return new StringBuilder().AppendPagingOuterQuery(SelectBuilder, _offset, countOnly)
                .AppendSelectAndDistinctClause(_distinct, countOnly)
                .AppendTopClause(OrderByBuilder, _offset, _pageSize, countOnly)
                .AppendCountOrSelectFields(SelectBuilder, countOnly)
                .AppendPagingPrefix(OrderByBuilder, _offset, countOnly)
                .AppendFromClause()
                .AppendTableName(FromBuilder)
                .AppendWhereClause(WhereBuilder, _offset, _pageSize)
                .AppendGroupByClause(GroupByBuilder)
                .AppendHavingClause(HavingBuilder)
                .AppendOrderByClause(OrderByBuilder, _offset, countOnly)
                .AppendPagingSuffix(_offset, _pageSize, countOnly)
                .ToString();
        }
    }
}
