using System.Data;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityMapper : IDatabaseEntityMapper<TestEntity>
    {
        public TestEntity Map(IDataReader reader)
            => new TestEntity
            (
                code: reader.GetString("Code"),
                codeType: reader.GetString("CodeType"),
                description: reader.GetNullableString("Description"),
                isExistingEntity: true
            );
    }
}
