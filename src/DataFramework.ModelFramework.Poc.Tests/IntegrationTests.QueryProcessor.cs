namespace DataFramework.ModelFramework.Poc.Tests;

public sealed partial class IntegrationTests
{
    [Fact]
    public async Task Can_Query_Database_Using_Basic_Query_Async()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var query = new CatalogQuery(new SingleEntityQueryBuilder()
            //.Where(nameof(Catalog.Name)).StartsWith("Diversen cd")
            .AddConditions(new StringStartsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithPropertyName(nameof(Catalog.Name)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("Diversen cd")))
            .Build());

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query, null, CancellationToken.None);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogDatabaseCommandEntityProvider
    }

    [Fact]
    public async Task Can_Query_Database_Using_ExtraFieldNames()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]") && cmd.CommandText.Contains(" WHERE ExtraField1 = @p0 "),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var query = new CatalogQuery(new SingleEntityQueryBuilder()
            //.Where("MyField").IsEqualTo("Value")
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithPropertyName("MyField"))
                .WithCompareExpression(new LiteralEvaluatableBuilder("Value")))
            .Build());

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query, null, CancellationToken.None);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
        actual.Value.First().ExtraField1.ShouldBe("Value");
    }

    [Fact]
    public async Task Can_Query_Database_Using_CustomQueryExpression()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains("WHERE CHARINDEX(@p0, [Name] + ' ' + [StartDirectory] + ' ' + COALESCE([ExtraField1], '') + ' ' + COALESCE([ExtraField2], '') + ' ' + COALESCE([ExtraField3], '') + ' ' + COALESCE([ExtraField4], '') + ' ' + COALESCE([ExtraField5], '') + ' ' + COALESCE([ExtraField6], '') + ' ' + COALESCE([ExtraField7], '') + ' ' + COALESCE([ExtraField8], '') + ' ' + COALESCE([ExtraField9], '') + ' ' + COALESCE([ExtraField10], '') + ' ' + COALESCE([ExtraField11], '') + ' ' + COALESCE([ExtraField12], '') + ' ' + COALESCE([ExtraField13], '') + ' ' + COALESCE([ExtraField14], '') + ' ' + COALESCE([ExtraField15], '') + ' ' + COALESCE([ExtraField16], '')) > 0"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var query = new CatalogQuery(new SingleEntityQueryBuilder()
            //.Where("AllFields").Contains("Diversen")
                .AddConditions(new StringContainsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithPropertyName("AllFields"))
                .WithCompareExpression(new LiteralEvaluatableBuilder("Diversen")))
            .Build());

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query, null, CancellationToken.None);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }

    [Fact]
    public async Task Can_Use_Internal_Expression_From_ExpressionFramework()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains("WHERE LEN([Name] + ' ' + [StartDirectory] + ' ' + COALESCE([ExtraField1], '') + ' ' + COALESCE([ExtraField2], '') + ' ' + COALESCE([ExtraField3], '') + ' ' + COALESCE([ExtraField4], '') + ' ' + COALESCE([ExtraField5], '') + ' ' + COALESCE([ExtraField6], '') + ' ' + COALESCE([ExtraField7], '') + ' ' + COALESCE([ExtraField8], '') + ' ' + COALESCE([ExtraField9], '') + ' ' + COALESCE([ExtraField10], '') + ' ' + COALESCE([ExtraField11], '') + ' ' + COALESCE([ExtraField12], '') + ' ' + COALESCE([ExtraField13], '') + ' ' + COALESCE([ExtraField14], '') + ' ' + COALESCE([ExtraField15], '') + ' ' + COALESCE([ExtraField16], '')) > @p0"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var query = new CatalogQuery(new SingleEntityQueryBuilder()
            //.Where("AllFields").Len().IsGreaterThan(4)
            .AddConditions(new GreaterThanConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithPropertyName("AllFields.Length"))
                .WithCompareExpression(new LiteralEvaluatableBuilder(4)))
            .Build());

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query, null, CancellationToken.None);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }
}
