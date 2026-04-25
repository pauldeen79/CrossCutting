namespace CrossCutting.Data.Abstractions;

public interface IEntityFieldInfo : IFieldNameProvider
{
    IEnumerable<string> GetAllFields();
}
