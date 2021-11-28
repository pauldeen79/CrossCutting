using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Core.Builders
{
    public class SelectCommandBuilder
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

        public SelectCommandBuilder Distinct(bool distinct = true)
        {
            _distinct = distinct;
            return this;
        }

        public SelectCommandBuilder Top(int top)
        {
            _top = top;
            return this;
        }

        public SelectCommandBuilder Select(IEnumerable<string> values)
            => Select(values.ToArray());

        public SelectCommandBuilder Select(params string[] values)
        {
            _selectBuilder.Append(string.Join(", ", values));
            return this;
        }

        public SelectCommandBuilder From(string value)
        {
            _fromBuilder.Append(value);
            return this;
        }

        public SelectCommandBuilder InnerJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
            }
            _fromBuilder.Append(" INNER JOIN ").Append(value);
            return this;
        }

        public SelectCommandBuilder LeftOuterJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            return this;
        }

        public SelectCommandBuilder RightOuterJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
            }
            _fromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            return this;
        }

        public SelectCommandBuilder CrossJoin(string value)
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
            }
            _fromBuilder.Append(" CROSS JOIN ").Append(value);
            return this;
        }

        public SelectCommandBuilder Where(string value)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
            return this;
        }

        public SelectCommandBuilder And(string value)
            => Where(value);

        public SelectCommandBuilder Or(string value)
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public SelectCommandBuilder OrderBy(string value)
        {
            _orderByBuilder.Append(value);
            return this;
        }

        public SelectCommandBuilder GroupBy(string value)
        {
            _groupByBuilder.Append(value);
            return this;
        }

        public SelectCommandBuilder Having(string value)
        {
            _havingBuilder.Append(value);
            return this;
        }

        public SelectCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public SelectCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public SelectCommandBuilder Clear()
        {
            _selectBuilder.Clear();
            _fromBuilder.Clear();
            _whereBuilder.Clear();
            _orderByBuilder.Clear();
            _groupByBuilder.Clear();
            _havingBuilder.Clear();
            CommandParameters.Clear();
            _distinct = false;
            _top = null;
            return this;
        }

        public SelectCommandBuilder AsStoredProcedure()
        {
            CommandType = DatabaseCommandType.StoredProcedure;
            return this;
        }

        public SelectCommandBuilder AsText()
        {
            CommandType = DatabaseCommandType.Text;
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), CommandType, DatabaseOperation.Select, CommandParameters);

        private string BuildSql()
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("FROM clause is missing");
            }

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
            builder.Append(_fromBuilder);
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
