namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityMapper : IDatabaseEntityMapper<TestEntity>
{
    public TestEntity Map(IDataReader reader)
        => new        (
            code: reader.GetString("Code"),
            codeType: reader.GetString("CodeType"),
            description: reader.GetNullableString("Description"),
            isExistingEntity: true
        );
}
