using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : Repository<TestEntity, TestEntityIdentity>
    {
        public TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                              IDatabaseEntityRetriever<TestEntity> entityRetriever,
                              IDatabaseCommandProvider<TestEntityIdentity> identitySelectCommandProvider,
                              IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                              IDatabaseCommandProvider entitySelectCommandProvider,
                              IDatabaseCommandProvider<TestEntity> entityCommandProvider)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
        }

        // for test purposes only. normally you would add arguments here (request/query)
        public TestEntity? FindOne()
            => EntityRetriever.FindOne(new SqlDatabaseCommand("SELECT * FROM MyTable WHERE ...", DatabaseCommandType.Text, DatabaseOperation.Select));

        // for test purposes only. normally you would add arguments here (request/query)
        public IReadOnlyCollection<TestEntity> FindMany()
            => EntityRetriever.FindMany(new SqlDatabaseCommand("SELECT * FROM MyTable WHERE ...", DatabaseCommandType.Text, DatabaseOperation.Select));

        // for test purposes only. normally you would add arguments here (request/query)
        public IPagedResult<TestEntity> FindPaged()
            => EntityRetriever.FindPaged(new PagedDatabaseCommand(new SqlDatabaseCommand("SELECT TOP 10 * FROM MyTable WHERE ...", DatabaseCommandType.Text, DatabaseOperation.Select),
                                                                  new SqlDatabaseCommand("SELECT COUNT(*) FROM MyTable WHERE ...", DatabaseCommandType.Text, DatabaseOperation.Select),
                                                                  0,
                                                                  10));
    }
}
