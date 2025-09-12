namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.Sql;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider>
{
    public class Create : QueryDatabaseCommandProviderTests
    {
        [Fact]
        public void Returns_Invalid_On_Non_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();

            // Act
            var result = sut.Create(query, DatabaseOperation.Insert);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Only select operation is supported");
        }

        [Fact]
        public void Returns_DatabaseCommand_On_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();

            // Act
            var command = sut.Create(query, DatabaseOperation.Select).EnsureValue().GetValueOrThrow();

            // Assert
            command.ShouldNotBeNull();
            command.CommandText.ShouldBe("SELECT * FROM MyEntity");
        }
    }
}
