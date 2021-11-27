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
        {
            var result = new TestEntityBuilder
            {
                Code = reader.GetString("Code"),
                CodeType = reader.GetString("CodeType"),
                Description = reader.GetNullableString("Description"),
                IsExistingEntity = true
            };
            
            // you can put some logic here to modify the result, if you want...

            return result.Build();
        }
    }
}
