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
        private readonly StringBuilder _selectBuilder;
        private readonly StringBuilder _fromBuilder;
        private readonly StringBuilder _whereBuilder;
        private readonly StringBuilder _orderByBuilder;
        private readonly StringBuilder _groupByBuilder;
        private readonly StringBuilder _havingBuilder;
        private bool _distinct;
        private int? _offset;
        private int? _pageSize;

        public PagedSelectCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _selectBuilder = new StringBuilder();
            _fromBuilder = new StringBuilder();
            _whereBuilder = new StringBuilder();
            _orderByBuilder = new StringBuilder();
            _groupByBuilder = new StringBuilder();
            _havingBuilder = new StringBuilder();
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
            _selectBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder From(string value)
        {
            _fromBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder InnerJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
            }
            _fromBuilder.Append(" INNER JOIN ").Append(value);
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
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder RightOuterJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder CrossJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
            }
            _fromBuilder.Append(" CROSS JOIN ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder Where(string value)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder And(string value)
            => Where(value);

        public PagedSelectCommandBuilder Or(string value)
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public PagedSelectCommandBuilder OrderBy(string value)
        {
            _orderByBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder GroupBy(string value)
        {
            _groupByBuilder.Append(value);
            return this;
        }

        public PagedSelectCommandBuilder Having(string value)
        {
            _havingBuilder.Append(value);
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
            _selectBuilder.Clear();
            _fromBuilder.Clear();
            _whereBuilder.Clear();
            _orderByBuilder.Clear();
            _groupByBuilder.Clear();
            _havingBuilder.Clear();
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
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("FROM clause is missing");
            }

            return new StringBuilder().AppendPagingOuterQuery(_selectBuilder, _offset, countOnly)
                .AppendSelectAndDistinctClause(_distinct, countOnly)
                .AppendTopClause(_orderByBuilder, _offset, _pageSize, countOnly)
                .AppendCountOrSelectFields(_selectBuilder, countOnly)
                .AppendPagingPrefix(_orderByBuilder, _offset, countOnly)
                .AppendFromClause()
                .AppendTableName(_fromBuilder)
                .AppendWhereClause(_whereBuilder, _offset, _pageSize)
                .AppendGroupByClause(_groupByBuilder)
                .AppendHavingClause(_havingBuilder)
                .AppendOrderByClause(_orderByBuilder, _offset, countOnly)
                .AppendPagingSuffix(_offset, _pageSize, countOnly)
                .ToString();
        }
    }
}
