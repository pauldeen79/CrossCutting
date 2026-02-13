namespace DataFramework.ModelFramework.Poc.Tests;

public sealed partial class IntegrationTests
{
    [Fact]
    public async Task Can_Find_Entity_By_Identity_Async()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });

        // Act
        var actual = await Repository.FindAsync(new CatalogIdentity(1));

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldNotBeNull();
        actual.Value.IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }

    [Fact]
    public async Task Can_Find_All_Entities_Async()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });

        // Act
        var actual = await Repository.FindAllAsync();

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }

    [Fact]
    public async Task Can_Add_Entity_To_Database_Async()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });

        // Act
        var actual = await Repository.AddAsync(new Catalog(0, "Test", DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "0000-0000", "CDT", "CDR", null, 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldNotBeNull();
        actual.Value.Id.ShouldBe(1); // read from database in CatalogDatabaseCommandEntityProvider
        actual.Value.IsExistingEntity.ShouldBeTrue(); //set from CatalogDatabaseCommandEntityProvider
    }

    [Fact]
    public async Task Can_Update_Entity_From_Database_Async()
    {
        // Arrange
        const string FindSql = "SELECT [Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], [DriveTypeCodeType], [DriveTypeCode], [DriveTypeDescription], [DriveTotalSize], [DriveFreeSpace], [Recursive], [Sorted], [StartDirectory], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16] FROM (SELECT c.[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], c.[DriveTypeCodeType], c.[DriveTypeCode], c.[DriveTotalSize], c.[DriveFreeSpace], c.[Recursive], c.[Sorted], c.[StartDirectory], c.[ExtraField1], c.[ExtraField2], c.[ExtraField3], c.[ExtraField4], c.[ExtraField5], c.[ExtraField6], c.[ExtraField7], c.[ExtraField8], c.[ExtraField9], c.[ExtraField10], c.[ExtraField11], c.[ExtraField12], c.[ExtraField13], c.[ExtraField14], c.[ExtraField15], c.[ExtraField16], cd.[Description] AS [DriveTypeDescription] FROM [Catalog] c INNER JOIN [Code] cd ON c.[DriveTypeCode] = cd.[Code] AND cd.[CodeType] = 'CDT') AS [CatalogView] WHERE [Id] = @Id";
        Connection.AddResultForDataReader(cmd => cmd.CommandText == FindSql && ((int)cmd.Parameters.Cast<DbDataParameter>().First().Value) == 1,
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1), "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        Connection.AddResultForDataReader(cmd => cmd.CommandText == "[UpdateCatalog]",
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Today, DateTime.Today, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var input = new CatalogBuilder((await Repository.FindAsync(new CatalogIdentity(1))).GetValueOrThrow()).WithExtraField1("value").Build();

        // Act
        var actual = await Repository.UpdateAsync(input);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldNotBeNull();
        actual.Value.Id.ShouldBe(1); // read from database in CatalogDatabaseCommandEntityProvider
        actual.Value.IsExistingEntity.ShouldBeTrue(); //set from CatalogDatabaseCommandEntityProvider
        actual.Value.ExtraField1.ShouldBe("Value"); //read from database in CatalogDatabaseCommandEntityProvider
    }

    [Fact]
    public async Task Can_Delete_Entity_From_Database_Async()
    {
        // Arrange
        const string FindSql = "SELECT [Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], [DriveTypeCodeType], [DriveTypeCode], [DriveTypeDescription], [DriveTotalSize], [DriveFreeSpace], [Recursive], [Sorted], [StartDirectory], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16] FROM (SELECT c.[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], c.[DriveTypeCodeType], c.[DriveTypeCode], c.[DriveTotalSize], c.[DriveFreeSpace], c.[Recursive], c.[Sorted], c.[StartDirectory], c.[ExtraField1], c.[ExtraField2], c.[ExtraField3], c.[ExtraField4], c.[ExtraField5], c.[ExtraField6], c.[ExtraField7], c.[ExtraField8], c.[ExtraField9], c.[ExtraField10], c.[ExtraField11], c.[ExtraField12], c.[ExtraField13], c.[ExtraField14], c.[ExtraField15], c.[ExtraField16], cd.[Description] AS [DriveTypeDescription] FROM [Catalog] c INNER JOIN [Code] cd ON c.[DriveTypeCode] = cd.[Code] AND cd.[CodeType] = 'CDT') AS [CatalogView] WHERE [Id] = @Id";
        Connection.AddResultForDataReader(cmd => cmd.CommandText == FindSql && ((int)cmd.Parameters.Cast<DbDataParameter>().First().Value) == 1,
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1), "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        Connection.AddResultForDataReader(cmd => cmd.CommandText == "[DeleteCatalog]",
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Today, DateTime.Today, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var input = new CatalogBuilder((await Repository.FindAsync(new CatalogIdentity(1))).GetValueOrThrow()).WithExtraField1("value").Build();

        // Act
        var actual = await Repository.DeleteAsync(input);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldNotBeNull();
        actual.Value.ExtraField1.ShouldBe("value"); //CatalogDatabaseCommandEntityProvider does not change value, this is a 'hard' delete
    }

    [Fact]
    public async Task Can_Use_QueryProcessor_In_Repository_Async()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [ExtraField]"),
                                          () => new[] { new ExtraField("Catalog", "MyField", null, 1, typeof(string).FullName, true) });

        // Act
        var actual = await Repository.FindSomethingAsync(CancellationToken.None);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }
}
