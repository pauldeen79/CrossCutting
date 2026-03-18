namespace DataFramework.ModelFramework.Poc.Tests;

public sealed partial class IntegrationTests
{
    [Fact]
    public async Task Can_Query_Database_Using_Command()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new CatalogBuilder()
            .WithId(1)
            .WithName("Diversen cd 1")
            .WithDateCreated(DateTime.Today)
            .WithDateLastModified(DateTime.Now)
            .WithDateSynchronized(DateTime.Now)
            .WithDriveSerialNumber("0000-0000")
            .WithDriveTypeCodeType("CDT")
            .WithDriveTypeCode("CDR")
            .WithDriveTypeDescription("CD-ROM")
            .WithDriveTotalSize(1)
            .WithDriveFreeSpace(2)
            .WithRecursive(true)
            .WithSorted(true)
            .WithStartDirectory(@"C:\").Build() });

        // Act
        var actual = await Retriever.FindManyAsync(new SelectCommandBuilder()
            .Select("*")
            .From("Catalog")
            .Where("LEFT(Name, 11) = @p0")
            .AppendParameter("p0", "Diversen cd")
            .Build());

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }
}
