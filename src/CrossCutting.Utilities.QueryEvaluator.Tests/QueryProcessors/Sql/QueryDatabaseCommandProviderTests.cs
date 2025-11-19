namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.Sql;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider>
{
    public class CreateAsync : QueryDatabaseCommandProviderTests
    {
        [Fact]
        public async Task Returns_Invalid_On_Non_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();

            // Act
            var result = await sut.CreateAsync(query, DatabaseOperation.Insert, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Only select operation is supported");
        }

        [Fact]
        public async Task Returns_DatabaseCommand_On_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();

            // Act
            var command = (await sut.CreateAsync(query, DatabaseOperation.Select, CancellationToken.None)).EnsureValue().GetValueOrThrow();

            // Assert
            command.ShouldNotBeNull();
            command.CommandText.ShouldBe("SELECT * FROM MyEntity");
        }
    }
}
