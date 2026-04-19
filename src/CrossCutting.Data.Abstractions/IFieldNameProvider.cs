namespace CrossCutting.Data.Abstractions;

public interface IFieldNameProvider
{
    string? GetDatabaseFieldName(string queryFieldName);
}
