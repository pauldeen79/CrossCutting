namespace CrossCutting.Data.Sql.Tests.Extensions;

public class DbCommandExtensionsTests
{
    [Fact]
    public void AddParameter_Creates_Parameter_Correctly()
    {
        // Arrange
        using var command = new DbCommand();

        // Act
        command.AddParameter("Name", "Value");

        // Assert
        var parameters = command.Parameters.Cast<IDbDataParameter>();
        parameters.Should().ContainSingle();
        parameters.First().ParameterName.Should().Be("Name");
        parameters.First().Value.Should().Be("Value");
    }

    [Fact]
    public void AddParameters_Add_All_Parameters_When_Argument_Is_Not_Null()
    {
        // Arrange
        using var command = new DbCommand();
        var parameters = new List<KeyValuePair<string, object?>>
        {
            new KeyValuePair<string, object?>("param1", "value1"),
            new KeyValuePair<string, object?>("param2", null),
        };

        // Act
        command.AddParameters(parameters);

        // Assert
        var result = command.Parameters.Cast<IDbDataParameter>();
        result.Should().HaveCount(2);
        result.First().ParameterName.Should().Be("param1");
        result.First().Value.Should().Be("value1");
        result.Last().ParameterName.Should().Be("param2");
        result.Last().Value.Should().Be(DBNull.Value);
    }

    [Fact]
    public void FillCommand_Fills_CommandText_Correctly_From_String()
    {
        // Arrange
        using var command = new DbCommand();

        // Act
        command.FillCommand($"Test", DatabaseCommandType.Text);

        // Assert
        command.CommandText.Should().Be("Test");
    }

    [Fact]
    public void FillCommand_Fills_CommandText_Correctly_From_FormattableString()
    {
        // Arrange
        using var command = new DbCommand();
        var value = "sql injection safe!";

        // Act
        command.FillCommand($"SELECT * FROM Table WHERE Field = {value}", DatabaseCommandType.Text);

        // Assert
        command.CommandText.Should().Be("SELECT * FROM Table WHERE Field = @p0");
    }

    [Theory]
    [InlineData(DatabaseCommandType.StoredProcedure, CommandType.StoredProcedure)]
    [InlineData(DatabaseCommandType.Text, CommandType.Text)]
    public void FillCommand_Fills_CommandType_Correctly(DatabaseCommandType dbCommandType, CommandType expectedCommandType)
    {
        // Arrange
        using var command = new DbCommand();

        // Act
        command.FillCommand($"Test", dbCommandType);

        // Assert
        command.CommandType.Should().Be(expectedCommandType);
    }

    [Fact]
    public void FillCommand_Fills_Parameters_Correctly_From_Parameters()
    {
        // Arrange
        using var command = new DbCommand();
        var parameters = new List<KeyValuePair<string, object?>>
        {
            new KeyValuePair<string, object?>("param1", "value1"),
            new KeyValuePair<string, object?>("param2", null),
        };

        // Act
        command.FillCommand("Test", DatabaseCommandType.Text, parameters);

        // Assert
        var result = command.Parameters.Cast<IDbDataParameter>();
        result.Should().HaveCount(2);
        result.First().ParameterName.Should().Be("param1");
        result.First().Value.Should().Be("value1");
        result.Last().ParameterName.Should().Be("param2");
        result.Last().Value.Should().Be(DBNull.Value);
    }

    [Fact]
    public void FillCommand_Fills_Parameters_Correctly_From_FormattableString()
    {
        // Arrange
        using var command = new DbCommand();
        var value = "sql injection safe!";

        // Act
        command.FillCommand($"SELECT * FROM Table WHERE Field = {value}", DatabaseCommandType.Text);

        // Assert
        var result = command.Parameters.Cast<IDbDataParameter>();
        result.Should().ContainSingle();
        result.First().ParameterName.Should().Be("p0");
        result.First().Value.Should().Be("sql injection safe!");
    }

    [Fact]
    public void FindOne_Returns_Correct_Result()
    {
        // Arrange
        using var connection = new DbConnection();
        connection.AddResultForDataReader(new[]
        {
            new MyDataObject { Property = "test" }
        });
        using var command = connection.CreateCommand();

        // Act
        var result = command.FindOne($"SELECT TOP 1 * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.Should().NotBeNull();
        result?.Property.Should().Be("test");
    }

    [Fact]
    public async Task FindOneAsync_Returns_Correct_Result()
    {
        // Arrange
        using var connection = new DbConnection();
        connection.AddResultForDataReader(new[]
        {
            new MyDataObject { Property = "test" }
        });
        using var command = connection.CreateCommand();
        var wrapperCommand = new SqlCommandWrapper(
            command,
            (cmd, _) => Task.FromResult(cmd.ExecuteNonQuery()),
            (cmd, _, _) => Task.FromResult(new SqlDataReaderWrapper(
                cmd.ExecuteReader(),
                (reader, _) => Task.FromResult(reader.Read()),
                (reader, _) => Task.FromResult(reader.NextResult()),
                (reader) => { reader.Close(); return Task.CompletedTask; })
            ),
            (cmd, _) => Task.FromResult(cmd.ExecuteScalar()));

        // Act
        var result = await wrapperCommand.FindOneAsync($"SELECT TOP 1 * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, CancellationToken.None, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.Should().NotBeNull();
        result?.Property.Should().Be("test");
    }

    [Fact]
    public void FindMany_Returns_Correct_Result()
    {
        // Arrange
        using var connection = new DbConnection();
        connection.AddResultForDataReader(new[]
        {
            new MyDataObject { Property = "test1" },
            new MyDataObject { Property = "test2" }
        });
        using var command = connection.CreateCommand();

        // Act
        var result = command.FindMany($"SELECT * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.Should().NotBeNull().And.HaveCount(2);
        result.First().Property.Should().Be("test1");
        result.Last().Property.Should().Be("test2");
    }

    [Fact]
    public async Task FindManyAsync_Returns_Correct_Result()
    {
        // Arrange
        using var connection = new DbConnection();
        connection.AddResultForDataReader(new[]
        {
            new MyDataObject { Property = "test1" },
            new MyDataObject { Property = "test2" }
        });
        using var command = connection.CreateCommand();
        var wrapperCommand = new SqlCommandWrapper(
            command,
            (cmd, _) => Task.FromResult(cmd.ExecuteNonQuery()),
            (cmd, _, _) => Task.FromResult(new SqlDataReaderWrapper(
                cmd.ExecuteReader(),
                (reader, _) => Task.FromResult(reader.Read()),
                (reader, _) => Task.FromResult(reader.NextResult()),
                (reader) => { reader.Close(); return Task.CompletedTask; })
            ),
            (cmd, _) => Task.FromResult(cmd.ExecuteScalar()));

        // Act
        var result = await wrapperCommand.FindManyAsync($"SELECT * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, CancellationToken.None, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.Should().NotBeNull().And.HaveCount(2);
        result.First().Property.Should().Be("test1");
        result.Last().Property.Should().Be("test2");
    }
    public class MyDataObject
    {
        public string? Property { get; set; }
    }
}
