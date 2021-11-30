using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class DatabaseEntityRetrieverSettings : IDatabaseEntityRetrieverSettings
    {
        public string TableName { get; }
        public string Fields { get; }
        public string DefaultOrderBy { get; }
        public string DefaultWhere { get; }

        public DatabaseEntityRetrieverSettings(string tableName, string fields, string defaultOrderBy, string defaultWhere)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
        }
    }
}
