namespace CrossCutting.Data.Core.Tests;

public class PagedDatabaseEntityRetrieverSettingsTests
{
    [Fact]
    public void Can_Construct()
    {
        // Act
        var sut = new PagedDatabaseEntityRetrieverSettings("table", "fields", "orderby", "where", 12345);

        // Assert
        sut.TableName.Should().Be("table");
        sut.Fields.Should().Be("fields");
        sut.DefaultOrderBy.Should().Be("orderby");
        sut.DefaultWhere.Should().Be("where");
        sut.OverridePageSize.Should().Be(12345);
    }
}
