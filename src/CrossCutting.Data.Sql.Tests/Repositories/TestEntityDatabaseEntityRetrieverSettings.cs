using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityDatabaseEntityRetrieverSettings : IDatabaseEntityRetrieverSettings
    {
        public string TableName => throw new System.NotImplementedException();

        public string Fields => throw new System.NotImplementedException();

        public string DefaultOrderBy => throw new System.NotImplementedException();

        public string DefaultWhere => throw new System.NotImplementedException();
    }
}
