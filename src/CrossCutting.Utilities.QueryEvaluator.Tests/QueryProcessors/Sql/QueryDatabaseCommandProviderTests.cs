namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.Sql;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider>
{
    public class Create : QueryDatabaseCommandProviderTests
    {
        [Fact]
        public void Throws_On_Non_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();
            Action a = () => sut.Create(query, DatabaseOperation.Insert);

            // Act & Assert
            a.ShouldThrow<ArgumentOutOfRangeException>()
             .Message.ShouldBe("Only select operation is supported (Parameter 'operation')");
        }

        [Fact]
        public void Returns_DatabaseCommand_On_Select_DatabaseOperation()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Build();
            var sut = CreateSut();

            // Act
            var command = sut.Create(query, DatabaseOperation.Select);

            // Assert
            command.ShouldNotBeNull();
            command.CommandText.ShouldBe("SELECT * FROM MyEntity");
        }
    }
}
