namespace CrossCutting.Data.Core.Tests.TestFixtures;

[ExcludeFromCodeCoverage]
public record TestEntity
{
    public bool IsExistingEntity { get; }
    public string Code { get; }
    public string CodeType { get; }
    public string? Description { get; }

    public TestEntity(string code, string codeType, string? description, bool isExistingEntity = false)
    {
        Code = code;
        CodeType = codeType;
        Description = description;
        IsExistingEntity = isExistingEntity;
    }
}
