namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseEntityRetrieverSettings : IPagedDatabaseEntityRetrieverSettings
{
    public string TableName => "MyTable";

    public string Fields => "Field1, Field2, Field3";

    public string DefaultOrderBy => string.Empty;

    public string DefaultWhere => string.Empty;

    public int? OverridePageSize => null;
}
