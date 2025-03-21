﻿namespace CrossCutting.Data.Sql.Tests.CommandProviders;

public class PagedSelectDatabaseCommandProviderTests : TestBase<PagedSelectDatabaseCommandProvider>
{
    private IPagedDatabaseEntityRetrieverSettings SettingsMock => Fixture.Freeze<IPagedDatabaseEntityRetrieverSettings>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void CreatePaged_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act
        Action a = () => Sut.CreatePaged<TestEntity>(operation, 1, 1);
        a.ShouldThrow<ArgumentOutOfRangeException>()
         .ParamName.ShouldBe("operation");
    }

    [Fact]
    public void CreatePaged_Throws_On_Unsupported_PagedDatabaseEntityRetrieverSettings()
    {
        // Act & Assert
        Action a = () => Sut.CreatePaged<TestEntity>(DatabaseOperation.Select, 0, 10);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Could not obtain paged database entity retriever settings for type [CrossCutting.Data.Sql.Tests.Repositories.TestEntity]");
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_First_Page()
    {
        // Arrange
        const string CommandSql = "SELECT TOP 10 Id, Active, Field1, Field2, Field3 FROM MyTable WHERE Active = 1 ORDER BY Field1";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.DefaultOrderBy.Returns("Field1");
        SettingsMock.DefaultWhere.Returns("Active = 1");
        SettingsMock.Fields.Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.TableName.Returns("MyTable");
        Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>()
               .TryGet<TestEntity>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings?>())
               .Returns(x => { x[0] = SettingsMock; return true; });

        // Act
        var actual = Sut.CreatePaged<TestEntity>(DatabaseOperation.Select, 0, 10);

        // Assert
        actual.DataCommand.CommandText.ShouldBe(CommandSql);
        actual.RecordCountCommand.CommandText.ShouldBe(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_Subsequent_Page_OrderBy_Specified()
    {
        // Arrange
        const string CommandSql = "SELECT Id, Active, Field1, Field2, Field3 FROM (SELECT Id, Active, Field1, Field2, Field3, ROW_NUMBER() OVER (ORDER BY Field1) as sq_row_number FROM MyTable WHERE Active = 1) sq WHERE sq.sq_row_number BETWEEN 11 and 20;";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.DefaultOrderBy.Returns("Field1");
        SettingsMock.DefaultWhere.Returns("Active = 1");
        SettingsMock.Fields.Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.TableName.Returns("MyTable");
        Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>()
               .TryGet<TestEntity>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings?>())
               .Returns(x => { x[0] = SettingsMock; return true; });


        // Act
        var actual = Sut.CreatePaged<TestEntity>(DatabaseOperation.Select, 10, 10);

        // Assert
        actual.DataCommand.CommandText.ShouldBe(CommandSql);
        actual.RecordCountCommand.CommandText.ShouldBe(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Returns_Correct_Command_On_Select_DatabaseOperation_Subsequent_Page_No_OrderBy_Specified()
    {
        // Arrange
        const string CommandSql = "SELECT Id, Active, Field1, Field2, Field3 FROM (SELECT TOP 10 Id, Active, Field1, Field2, Field3, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number FROM MyTable WHERE Active = 1) sq WHERE sq.sq_row_number BETWEEN 11 and 20;";
        const string RecordCountSql = "SELECT COUNT(*) FROM MyTable WHERE Active = 1";
        SettingsMock.DefaultWhere.Returns("Active = 1");
        SettingsMock.Fields.Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.TableName.Returns("MyTable");
        Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>()
               .TryGet<TestEntity>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings?>())
               .Returns(x => { x[0] = SettingsMock; return true; });

        // Act
        var actual = Sut.CreatePaged<TestEntity>(DatabaseOperation.Select, 10, 10);

        // Assert
        actual.DataCommand.CommandText.ShouldBe(CommandSql);
        actual.RecordCountCommand.CommandText.ShouldBe(RecordCountSql);
    }

    [Fact]
    public void CreatePaged_Limits_PageSize_Based_On_Settings()
    {
        // Arrange
        SettingsMock.TableName.Returns("MyTable");
        SettingsMock.OverridePageSize.Returns(100);
        Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>()
               .TryGet<TestEntity>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings?>())
               .Returns(x => { x[0] = SettingsMock; return true; });

        // Act
        var actual = Sut.CreatePaged<TestEntity>(DatabaseOperation.Select, 0, 1000);

        // Assert
        actual.PageSize.ShouldBe(100);
    }
}
