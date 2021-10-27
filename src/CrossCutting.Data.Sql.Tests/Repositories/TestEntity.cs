using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public bool IsExistingEntity { get; set; }
        public string Code { get; set; }
        public string CodeType { get; set; }
        public string Description { get; set; }

        public TestEntity(string code, string codeType, string description, bool isExistingEntity = false)
        {
            Code = code;
            CodeType = codeType;
            Description = description;
            IsExistingEntity = isExistingEntity;
        }
    }
}
