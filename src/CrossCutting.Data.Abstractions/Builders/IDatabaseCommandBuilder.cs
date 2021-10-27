using System.Collections.Generic;

namespace CrossCutting.Data.Abstractions.Builders
{
    public interface IDatabaseCommandBuilder
    {
        DatabaseCommandType CommandType { get; set; }
        IDictionary<string, object> CommandParameters { get; set; }

        IDatabaseCommandBuilder Append(string value);
        IDatabaseCommandBuilder AppendParameter(string key, object value);
        IDatabaseCommand Build();
        IDatabaseCommandBuilder Clear();
    }
}
