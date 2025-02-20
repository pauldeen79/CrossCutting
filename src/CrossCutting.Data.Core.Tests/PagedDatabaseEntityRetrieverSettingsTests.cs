namespace CrossCutting.Data.Core.Tests;

public class PagedDatabaseEntityRetrieverSettingsTests
{
    [Fact]
    public void Can_Construct()
    {
        // Act
        var sut = new PagedDatabaseEntityRetrieverSettings("table", "fields", "orderby", "where", 12345);

        // Assert
        sut.TableName.ShouldBe("table");
        sut.Fields.ShouldBe("fields");
        sut.DefaultOrderBy.ShouldBe("orderby");
        sut.DefaultWhere.ShouldBe("where");
        sut.OverridePageSize.ShouldBe(12345);
    }
}
