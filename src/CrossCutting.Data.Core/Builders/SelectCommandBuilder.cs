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
        public IDictionary<string, object> CommandParameters { get; set; }
        private readonly StringBuilder _selectBuilder;
        private readonly StringBuilder _fromBuilder;
        private readonly StringBuilder _whereBuilder;
        private readonly StringBuilder _orderByBuilder;
        private readonly StringBuilder _groupByBuilder;
        private readonly StringBuilder _havingBuilder;
        public int? Top { get; set; }
        public bool Distinct { get; set; }

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

        public SelectCommandBuilder DistinctValues(bool distinct = true)
            => this.Chain(() => Distinct = distinct);

        public SelectCommandBuilder WithTop(int top)
            => this.Chain(() => Top = top);

        public SelectCommandBuilder Select(IEnumerable<string> values)
            => Select(values.ToArray());

        public SelectCommandBuilder Select(params string[] values)
            => this.Chain(() => _selectBuilder.Append(string.Join(", ", values)));

        public SelectCommandBuilder From(string value)
            => this.Chain(() => _fromBuilder.Append(value));

        public SelectCommandBuilder InnerJoin(string value)
            => this.Chain(() =>
            {
                if (_fromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add INNER JOIN clause to");
                }
                _fromBuilder.Append(" INNER JOIN ").Append(value);
            });

        public SelectCommandBuilder LeftOuterJoin(string value)
            => this.Chain(() =>
            {
                if (_fromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add LEFT OUTER JOIN clause to");
                }
                _fromBuilder.Append(" LEFT OUTER JOIN ").Append(value);
            });

        public SelectCommandBuilder RightOuterJoin(string value)
            => this.Chain(() =>
            {
                if (_fromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add RIGHT OUTER JOIN clause to");
                }
                _fromBuilder.Append(" RIGHT OUTER JOIN ").Append(value);
            });

        public SelectCommandBuilder CrossJoin(string value)
            => this.Chain(() =>
            {
                if (_fromBuilder.Length == 0)
                {
                    throw new InvalidOperationException("No FROM clause found to add CROSS JOIN clause to");
                }
                _fromBuilder.Append(" CROSS JOIN ").Append(value);
            });

        public SelectCommandBuilder Where(string value)
            => this.Chain(() =>
            {
                if (_whereBuilder.Length > 0)
                {
                    _whereBuilder.Append(" AND ");
                }
                _whereBuilder.Append(value);
            });

        public SelectCommandBuilder And(string value)
            => Where(value);

        public SelectCommandBuilder Or(string value)
            => this.Chain(() =>
            {
                if (_whereBuilder.Length == 0)
                {
                    throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
                }
                _whereBuilder.Append(" OR ").Append(value);
            });

        public SelectCommandBuilder OrderBy(string value)
            => this.Chain(() => _orderByBuilder.Append(value));

        public SelectCommandBuilder GroupBy(string value)
            => this.Chain(() => _groupByBuilder.Append(value));

        public SelectCommandBuilder Having(string value)
            => this.Chain(() => _havingBuilder.Append(value));

        public SelectCommandBuilder AppendParameter(string key, object value)
            => this.Chain(() => CommandParameters.Add(key, value));

        public SelectCommandBuilder AppendParameters(object parameters)
            => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

        public SelectCommandBuilder Clear()
        {
            _selectBuilder.Clear();
            _fromBuilder.Clear();
            _whereBuilder.Clear();
            _orderByBuilder.Clear();
            _groupByBuilder.Clear();
            _havingBuilder.Clear();
            CommandParameters.Clear();
            Distinct = false;
            Top = null;
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Select, CommandParameters);

        private string BuildSql()
        {
            if (_fromBuilder.Length == 0)
            {
                throw new InvalidOperationException("FROM clause is missing");
            }

            var builder = new StringBuilder();

            builder.Append("SELECT ");
            if (Distinct)
            {
                builder.Append("DISTINCT ");
            }
            if (Top != null)
            {
                builder.Append("TOP ")
                       .Append(Top.Value)
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
