namespace CrossCutting.Data.Sql.Tests.CommandProviders;

public class PagedSelectDatabaseCommandProviderTests : TestBase<PagedSelectDatabaseCommandProvider>
{
    private Mock<IPagedDatabaseEntityRetrieverSettings> SettingsMock => Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettings>>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void CreatePaged_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act
        Sut.Invoking(x => x.CreatePaged(operation, 1, 1))
           .Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("operation");
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_First_Page()
    {
        // Arrange
        const string CommandSql = "SELECT TOP 10 Id, Active, Field1, Field2, Field3 FROM MyTable WHERE Active = 1 ORDER BY Field1";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Field1");
        SettingsMock.SetupGet(x => x.DefaultWhere).Returns("Active = 1");
        SettingsMock.SetupGet(x => x.Fields).Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.SetupGet(x => x.TableName).Returns("MyTable");

        // Act
        var actual = Sut.CreatePaged(DatabaseOperation.Select, 0, 10);

        // Assert
        actual.DataCommand.CommandText.Should().Be(CommandSql);
        actual.RecordCountCommand.CommandText.Should().Be(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_Subsequent_Page_OrderBy_Specified()
    {
        // Arrange
        const string CommandSql = "SELECT Id, Active, Field1, Field2, Field3 FROM (SELECT Id, Active, Field1, Field2, Field3, ROW_NUMBER() OVER (ORDER BY Field1) as sq_row_number FROM MyTable WHERE Active = 1) sq WHERE sq.sq_row_number BETWEEN 11 and 20;";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Field1");
        SettingsMock.SetupGet(x => x.DefaultWhere).Returns("Active = 1");
        SettingsMock.SetupGet(x => x.Fields).Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.SetupGet(x => x.TableName).Returns("MyTable");

        // Act
        var actual = Sut.CreatePaged(DatabaseOperation.Select, 10, 10);

        // Assert
        actual.DataCommand.CommandText.Should().Be(CommandSql);
        actual.RecordCountCommand.CommandText.Should().Be(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_Subsequent_Page_No_OrderBy_Specified()
    {
        // Arrange
        const string CommandSql = "SELECT Id, Active, Field1, Field2, Field3 FROM (SELECT TOP 10 Id, Active, Field1, Field2, Field3, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number FROM MyTable WHERE Active = 1) sq WHERE sq.sq_row_number BETWEEN 11 and 20;";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.SetupGet(x => x.DefaultWhere).Returns("Active = 1");
        SettingsMock.SetupGet(x => x.Fields).Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.SetupGet(x => x.TableName).Returns("MyTable");

        // Act
        var actual = Sut.CreatePaged(DatabaseOperation.Select, 10, 10);

        // Assert
        actual.DataCommand.CommandText.Should().Be(CommandSql);
        actual.RecordCountCommand.CommandText.Should().Be(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Limits_PageSize_Based_On_Settings()
    {
        // Arrange
        SettingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        SettingsMock.SetupGet(x => x.OverridePageSize).Returns(100);

        // Act
        var actual = Sut.CreatePaged(DatabaseOperation.Select, 0, 1000);

        // Assert
        actual.PageSize.Should().Be(100);
    }
}
