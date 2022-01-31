namespace CrossCutting.Data.Core.Tests;

[ExcludeFromCodeCoverage]
public class DatabaseEntityRetrieverSettingsTests
{
    [Fact]
    public void Can_Construct()
    {
        // Act
        var sut = new DatabaseEntityRetrieverSettings("table", "fields", "orderby", "where");

        // Assert
        sut.TableName.Should().Be("table");
        sut.Fields.Should().Be("fields");
        sut.DefaultOrderBy.Should().Be("orderby");
        sut.DefaultWhere.Should().Be("where");
    }
}
