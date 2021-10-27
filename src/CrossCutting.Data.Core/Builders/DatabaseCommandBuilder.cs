using System.Collections.Generic;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Builders;

namespace CrossCutting.Data.Core.Builders
{
    public class DatabaseCommandBuilder : IDatabaseCommandBuilder
    {
        public DatabaseCommandType CommandType { get; set; }
        public IDictionary<string, object> CommandParameters { get; set; }
        private readonly StringBuilder _commandTextBuilder;

        public DatabaseCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _commandTextBuilder = new StringBuilder();
        }

        public IDatabaseCommandBuilder Append(string value)
        {
            _commandTextBuilder.Append(value);
            return this;
        }

        public IDatabaseCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public IDatabaseCommandBuilder Clear()
        {
            _commandTextBuilder.Clear();
            CommandParameters.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDbCommand(_commandTextBuilder.ToString(), CommandType, CommandParameters);
    }
}
