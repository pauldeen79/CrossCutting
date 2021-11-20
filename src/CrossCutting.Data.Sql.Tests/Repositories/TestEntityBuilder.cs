namespace CrossCutting.Data.Sql.Tests.Repositories
{
    public class TestEntityBuilder
    {
        public bool IsExistingEntity { get; set; }
        public string Code { get; set; }
        public string CodeType { get; set; }
        public string? Description { get; set; }

        public TestEntityBuilder()
        {
            Code = string.Empty;
            CodeType = string.Empty;
        }

        public TestEntityBuilder(TestEntity source)
        {
            IsExistingEntity = source.IsExistingEntity;
            Code = source.Code;
            CodeType = source.CodeType;
            Description = source.Description;
        }

        public TestEntity Build()
        {
            return new TestEntity(Code, CodeType, Description, IsExistingEntity);
        }
    }
}
