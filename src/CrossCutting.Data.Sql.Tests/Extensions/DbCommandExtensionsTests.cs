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
        parameters.Count().ShouldBe(1);
        parameters.First().ParameterName.ShouldBe("Name");
        parameters.First().Value.ShouldBe("Value");
    }

    [Fact]
    public void AddParameters_Add_All_Parameters_When_Argument_Is_Not_Null()
    {
        // Arrange
        using var command = new DbCommand();
        var parameters = new List<KeyValuePair<string, object?>>
        {
            new("param1", "value1"),
            new("param2", null),
        };

        // Act
        command.AddParameters(parameters);

        // Assert
        var result = command.Parameters.Cast<IDbDataParameter>();
        result.Count().ShouldBe(2);
        result.First().ParameterName.ShouldBe("param1");
        result.First().Value.ShouldBe("value1");
        result.Last().ParameterName.ShouldBe("param2");
        result.Last().Value.ShouldBe(DBNull.Value);
    }

    [Fact]
    public void FillCommand_Fills_CommandText_Correctly_From_String()
    {
        // Arrange
        using var command = new DbCommand();

        // Act
        command.FillCommand($"Test", DatabaseCommandType.Text);

        // Assert
        command.CommandText.ShouldBe("Test");
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
        command.CommandText.ShouldBe("SELECT * FROM Table WHERE Field = @p0");
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
        command.CommandType.ShouldBe(expectedCommandType);
    }

    [Fact]
    public void FillCommand_Fills_Parameters_Correctly_From_Parameters()
    {
        // Arrange
        using var command = new DbCommand();
        var parameters = new List<KeyValuePair<string, object?>>
        {
            new("param1", "value1"),
            new("param2", null),
        };

        // Act
        command.FillCommand("Test", DatabaseCommandType.Text, parameters);

        // Assert
        var result = command.Parameters.Cast<IDbDataParameter>();
        result.Count().ShouldBe(2);
        result.First().ParameterName.ShouldBe("param1");
        result.First().Value.ShouldBe("value1");
        result.Last().ParameterName.ShouldBe("param2");
        result.Last().Value.ShouldBe(DBNull.Value);
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
        result.Count().ShouldBe(1);
        result.First().ParameterName.ShouldBe("p0");
        result.First().Value.ShouldBe("sql injection safe!");
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
        result.ShouldNotBeNull();
        result.Property.ShouldBe("test");
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

        // Act
        var result = await command.FindOneAsync($"SELECT TOP 1 * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, CancellationToken.None, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.ShouldNotBeNull();
        result.Property.ShouldBe("test");
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
        result.ShouldNotBeNull().Count.ShouldBe(2);
        result.First().Property.ShouldBe("test1");
        result.Last().Property.ShouldBe("test2");
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

        // Act
        var result = await command.FindManyAsync($"SELECT * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, CancellationToken.None, reader => new MyDataObject { Property = reader.GetString("Property") });

        // Assert
        result.ShouldNotBeNull().Count.ShouldBe(2);
        result.First().Property.ShouldBe("test1");
        result.Last().Property.ShouldBe("test2");
    }
    public class MyDataObject
    {
        public string? Property { get; set; }
    }
}
