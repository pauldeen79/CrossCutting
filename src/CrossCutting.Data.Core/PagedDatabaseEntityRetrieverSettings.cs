using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class PagedDatabaseEntityRetrieverSettings : IPagedDatabaseEntityRetrieverSettings
    {
        public int? OverridePageSize { get; }
        public string TableName { get; }
        public string Fields { get; }
        public string DefaultOrderBy { get; }
        public string DefaultWhere { get; }

        public PagedDatabaseEntityRetrieverSettings(string tableName, string fields, string defaultOrderBy, string defaultWhere, int? overridePageSize)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
            OverridePageSize = overridePageSize;
        }
    }
}
