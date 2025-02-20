namespace CrossCutting.Data.Core.Tests;

public class DatabaseEntityRetrieverSettingsTests
{
    [Fact]
    public void Can_Construct()
    {
        // Act
        var sut = new DatabaseEntityRetrieverSettings("table", "fields", "orderby", "where");

        // Assert
        sut.TableName.ShouldBe("table");
        sut.Fields.ShouldBe("fields");
        sut.DefaultOrderBy.ShouldBe("orderby");
        sut.DefaultWhere.ShouldBe("where");
    }
}
