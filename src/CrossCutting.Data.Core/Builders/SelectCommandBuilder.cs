using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Builders;

namespace CrossCutting.Data.Core.Builders
{
    public class SelectCommandBuilder : ISelectCommandBuilder
    {
        public DatabaseCommandType CommandType { get; set; }
        public IDictionary<string, object> CommandParameters { get; set; }
        private readonly StringBuilder _selectBuilder;
        private readonly StringBuilder _fromBuilder;
        private readonly StringBuilder _whereBuilder;
        private readonly StringBuilder _orderByBuilder;
        private readonly StringBuilder _groupByBuilder;
        private readonly StringBuilder _havingBuilder;
        private int? _top;
        private bool _distinct;

        public SelectCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _selectBuilder = new StringBuilder();
            _fromBuilder = new StringBuilder();
            _whereBuilder = new StringBuilder();
            _orderByBuilder = new StringBuilder();
            _groupByBuilder = new StringBuilder();
            _havingBuilder = new StringBuilder();
        }

        public ISelectCommandBuilder Distinct(bool distinct = true)
        {
            _distinct = distinct;
            return this;
        }

        public ISelectCommandBuilder Top(int top)
        {
            _top = top;
            return this;
        }

        public ISelectCommandBuilder Select(string value)
        {
            _selectBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder From(string value)
        {
            _fromBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder InnerJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
            }
            _fromBuilder.Append(" INNER JOIN ").Append(value);
            return this;
        }

        public ISelectCommandBuilder LeftOuterJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            return this;
        }

        public ISelectCommandBuilder RightOuterJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            return this;
        }

        public ISelectCommandBuilder CrossJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
            }
            _fromBuilder.Append(" CROSS JOIN ").Append(value);
            return this;
        }

        public ISelectCommandBuilder Where(string value)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder And(string value)
            => Where(value);

        public ISelectCommandBuilder Or(string value)
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public ISelectCommandBuilder OrderBy(string value)
        {
            _orderByBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder GroupBy(string value)
        {
            _groupByBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder Having(string value)
        {
            _havingBuilder.Append(value);
            return this;
        }

        public ISelectCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public ISelectCommandBuilder Clear()
        {
            _selectBuilder.Clear();
            _fromBuilder.Clear();
            _whereBuilder.Clear();
            _orderByBuilder.Clear();
            _groupByBuilder.Clear();
            _havingBuilder.Clear();
            CommandParameters.Clear();
            return this;
        }

        public ISelectCommandBuilder AsStoredProcedure()
        {
            CommandType = DatabaseCommandType.StoredProcedure;
            return this;
        }

        public ISelectCommandBuilder AsText()
        {
            CommandType = DatabaseCommandType.Text;
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDbCommand(BuildSql(), CommandType, CommandParameters);

        private string BuildSql()
        {
            var builder = new StringBuilder();

            builder.Append("SELECT ");
            if (_distinct)
            {
                builder.Append("DISTINCT ");
            }
            if (_top != null)
            {
                builder.Append("TOP ")
                       .Append(_top.Value)
                       .Append(" ");
            }
            if (_selectBuilder.Length > 0)
            {
                builder.Append(_selectBuilder);
            }
            else
            {
                builder.Append("*");
            }
            builder.Append(" FROM ");
            if (_fromBuilder.Length > 0)
            {
                builder.Append(_fromBuilder);
            }
            else
            {
                throw new InvalidOperationException("FROM clause is missing");
            }
            if (_whereBuilder.Length > 0)
            {
                builder.Append(" WHERE ").Append(_whereBuilder);
            }
            if (_orderByBuilder.Length > 0)
            {
                builder.Append(" ORDER BY ").Append(_orderByBuilder);
            }
            if (_groupByBuilder.Length > 0)
            {
                builder.Append(" GROUP BY ").Append(_groupByBuilder);
            }
            if (_havingBuilder.Length > 0)
            {
                builder.Append(" HAVING ").Append(_havingBuilder);
            }
            return builder.ToString();
        }
    }
}
