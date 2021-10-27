using System.Collections.Generic;

namespace CrossCutting.Data.Abstractions.Builders
{
    public interface ISelectCommandBuilder
    {
        DatabaseCommandType CommandType { get; set; }
        IDictionary<string, object> CommandParameters { get; set; }

        ISelectCommandBuilder And(string value);
        ISelectCommandBuilder AppendParameter(string key, object value);
        ISelectCommandBuilder AsStoredProcedure();
        ISelectCommandBuilder AsText();
        IDatabaseCommand Build();
        ISelectCommandBuilder Clear();
        ISelectCommandBuilder CrossJoin(string value);
        ISelectCommandBuilder From(string value);
        ISelectCommandBuilder GroupBy(string value);
        ISelectCommandBuilder Having(string value);
        ISelectCommandBuilder InnerJoin(string value);
        ISelectCommandBuilder LeftOuterJoin(string value);
        ISelectCommandBuilder Or(string value);
        ISelectCommandBuilder OrderBy(string value);
        ISelectCommandBuilder RightOuterJoin(string value);
        ISelectCommandBuilder Select(string value);
        ISelectCommandBuilder Top(int top);
        ISelectCommandBuilder Where(string value);
    }
}
