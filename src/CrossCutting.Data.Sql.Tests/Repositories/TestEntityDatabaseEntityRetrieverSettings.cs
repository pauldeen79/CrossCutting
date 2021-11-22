using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityDatabaseEntityRetrieverSettings : IDatabaseEntityRetrieverSettings
    {
        public string TableName => "MyTable";

        public string Fields => "Field1, Field2, Field3";

        public string DefaultOrderBy => string.Empty;

        public string DefaultWhere => string.Empty;
    }
}
