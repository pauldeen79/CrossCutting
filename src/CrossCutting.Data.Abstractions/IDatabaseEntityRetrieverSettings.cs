namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseEntityRetrieverSettings
    {
        string TableName { get; }
        string Fields { get; }
        string DefaultOrderBy { get; }
        string DefaultWhere { get; }
    }
}
