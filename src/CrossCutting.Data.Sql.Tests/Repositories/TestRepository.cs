using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Sql.Builders;

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
            => EntityRetriever.FindOne(new SelectCommandBuilder()
                .Select("*")
                .WithTop(1)
                .From("MyTable")
                .Where("Field = Value")
                .Build());

        // for test purposes only. normally you would add arguments here (request/query)
        public IReadOnlyCollection<TestEntity> FindMany(string value)
            => EntityRetriever.FindMany(new SelectCommandBuilder()
                .Select("*")
                .From("MyTable")
                .Where("Field = @Value")
                .AppendParameter("Value", value)
                .Build());

        // for test purposes only. normally you would add arguments here (request/query)
        public IPagedResult<TestEntity> FindPaged(int offset, int pageSize)
            => EntityRetriever.FindPaged(new PagedSelectCommandBuilder()
                .Select("*")
                .From("MyTable")
                .OrderBy("Name")
                .Skip(offset)
                .Take(pageSize)
                .Build());
    }
}
