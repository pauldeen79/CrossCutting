using System.Collections.Generic;
using System.Text;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core.Builders
{
    public class DatabaseCommandBuilder
    {
        public DatabaseCommandType CommandType { get; set; }
        public IDictionary<string, object> CommandParameters { get; set; }
        private readonly StringBuilder _commandTextBuilder;

        public DatabaseCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _commandTextBuilder = new StringBuilder();
        }

        public DatabaseCommandBuilder Append(string value)
        {
            _commandTextBuilder.Append(value);
            return this;
        }

        public DatabaseCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public DatabaseCommandBuilder Clear()
        {
            _commandTextBuilder.Clear();
            CommandParameters.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(_commandTextBuilder.ToString(), CommandType, CommandParameters);
    }
}
