namespace DataFramework.ModelFramework.Poc.Tests;

public sealed partial class IntegrationTests
{
    [Fact]
    public async Task Can_Query_Database_Using_Basic_Query()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        // Note that you might want to cast to IQuery, but that is not possible due to C# language restrictions, at this time... (no implicit operator allowed on interfaces)
        CatalogQuery query = new CatalogQueryBuilder()
            //.Where(nameof(Catalog.Name)).StartsWith("Diversen cd")
            .Where(new StringStartsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(Catalog.Name)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("Diversen cd")));

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogDatabaseCommandEntityProvider
    }

    [Fact]
    public async Task Can_Query_Database_Using_Basic_Query_Using_QueryParser()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var parser = new QueryParser<IQueryBuilder, PropertyNameEvaluatableBuilder>(() => new PropertyNameEvaluatableBuilder("PrefilledField"));
        var query = parser.Parse(new CatalogQueryBuilder(), $"Name = \"Diversen cd 1\"").Build();

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogDatabaseCommandEntityProvider
    }

    [Fact]
    public async Task Can_Query_Database_Using_ExtraFieldNames_Literal()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]") && cmd.CommandText.Contains(" WHERE [ExtraField1] = @p0 "),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        // Note that you can use 'var' here as well, which uses QueryBase instead of IQuery (where QueryBase implements IQuery)
        IQuery query = new CatalogQueryBuilder()
            .Where("MyField").IsEqualTo("Value")
            // .Where(new EqualConditionBuilder()
            //     .WithSourceExpression(new PropertyNameEvaluatableBuilder("MyField"))
            //     .WithCompareExpression(new LiteralEvaluatableBuilder("Value")))
            .Build();

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
        actual.Value.First().ExtraField1.ShouldBe("Value");
    }

    [Fact]
    public async Task Can_Query_Database_Using_ExtraFieldNames_Delegate()
    {
        // Arrange
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [Catalog]") && cmd.CommandText.Contains(" WHERE [ExtraField1] = @p0 "),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", "Value", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        // Note that you can use 'var' here as well, which uses QueryBase instead of IQuery (where QueryBase implements IQuery)
        IQuery query = new CatalogQueryBuilder()
            .Where("MyField").IsEqualTo(new DelegateEvaluatableBuilder<string>(() => "Value"))
            .Build();

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query);

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
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains("WHERE [Name] + ' ' + [StartDirectory] + ' ' + COALESCE([ExtraField1], '') + ' ' + COALESCE([ExtraField2], '') + ' ' + COALESCE([ExtraField3], '') + ' ' + COALESCE([ExtraField4], '') + ' ' + COALESCE([ExtraField5], '') + ' ' + COALESCE([ExtraField6], '') + ' ' + COALESCE([ExtraField7], '') + ' ' + COALESCE([ExtraField8], '') + ' ' + COALESCE([ExtraField9], '') + ' ' + COALESCE([ExtraField10], '') + ' ' + COALESCE([ExtraField11], '') + ' ' + COALESCE([ExtraField12], '') + ' ' + COALESCE([ExtraField13], '') + ' ' + COALESCE([ExtraField14], '') + ' ' + COALESCE([ExtraField15], '') + ' ' + COALESCE([ExtraField16], '') LIKE @p0"),
                                          () => new[] { new Catalog(1, "Diversen cd 1", DateTime.Today, DateTime.Now, DateTime.Now, "0000-0000", "CDT", "CDR", "CD-ROM", 1, 2, true, true, @"C:\", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) });
        var query = new CatalogQuery(new SingleEntityQueryBuilder()
            //.Where("AllFields").Contains("Diversen")
            .Where(new StringContainsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder("AllFields"))
                .WithCompareExpression(new LiteralEvaluatableBuilder("Diversen")))
            .Build());

        // Act
        var actual = await QueryProcessor.FindManyAsync<Catalog>(query);

        // Assert
        actual.Status.ShouldBe(CrossCutting.Common.Results.ResultStatus.Ok);
        actual.Value.ShouldHaveSingleItem();
        actual.Value.First().IsExistingEntity.ShouldBeTrue(); //set from CatalogEntityMapper
    }
}
