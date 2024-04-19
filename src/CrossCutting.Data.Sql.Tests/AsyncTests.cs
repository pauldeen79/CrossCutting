namespace CrossCutting.Data.Sql.Tests;

public class AsyncTests
{
    [Fact(Skip = "Just a piece of code to check how to work with async stuff on Sql connections")]
    public void Should_Be_Able_To_Do_Stuff_Async()
    {
        // Arrange
        using var conn = new SqlConnection();
        using var cmd = conn.CreateCommand();

        cmd.ExecuteReaderAsync();
        cmd.ExecuteNonQueryAsync();
        cmd.ExecuteScalarAsync();
    }
}
