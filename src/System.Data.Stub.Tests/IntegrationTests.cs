namespace System.Data.Stub.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly DbConnection Connection;
    private readonly DbConnectionCallback _callback;

    public IntegrationTests()
    {
        _callback = new DbConnectionCallback();
        Connection = new DbConnection().AddCallback(_callback);
    }

    [Fact]
    public async Task Can_Stub_Command_For_ExecuteNonQuery()
    {
        // Arrange
        using var command = Connection.CreateCommand();
        _callback.Commands.Last().ExecuteNonQueryResult = 12345;

        // Act
        command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
        var actual = await command.ExecuteNonQueryAsync();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public async Task Can_Stub_Command_For_ExecuteScalar()
    {
        // Arrange
        using var command = Connection.CreateCommand();
        _callback.Commands.Last().ExecuteScalarResult = 2;

        // Act
        command.CommandText = "SELCT MAX(Amount) FROM [Fridge]";
        var actual = await command.ExecuteScalarAsync();

        // Assert
        actual.Should().Be(2);
    }

    [Fact]
    public async Task Can_Stub_Command_For_ExecuteReader()
    {
        await ReaderTest(reader => new MyRecord
        {
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
        });
    }

    [Fact]
    public async Task Can_Use_Indexer_On_DataReader()
    {
        await ReaderTest(reader => new MyRecord
        {
            Name = (string)reader["Name"],
            Amount = (int)reader["Amount"]
        });
    }

    [Fact]
    public async Task Can_Use_GetValue_And_GetOrdinal_On_DataReader()
    {
        await ReaderTest(reader => new MyRecord
        {
            Name = (string)reader.GetValue(reader.GetOrdinal("Name")),
            Amount = (int)reader.GetValue(reader.GetOrdinal("Amount"))
        });
    }

    [Fact]
    public async Task Can_Use_GetValue_And_ColumnIndex_On_DataReader()
    {
        await ReaderTest(reader => new MyRecord
        {
            Name = (string)reader.GetValue(0),
            Amount = (int)reader.GetValue(1)
        });
    }

    [Fact]
    public async Task Can_Call_GetValues_On_DataReader()
    {
        // Arrange & Act
        int result = int.MinValue;
        object[] values = new object[2];
        await ReaderTest(reader =>
        {
            result = reader.GetValues(values);
            return new MyRecord
            {
                Amount = (int)values[1],
                Name = (string)values[0]
            };
        });
    }

    [Fact]
    public void Can_Get_FieldCount_On_DataReader()
    {
        // Arrange
        // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
        Connection.AddResultForDataReader(new[]
        {
                new MyRecord { Name = "Beer", Amount = 1 },
                new MyRecord { Name = "Milk", Amount = 3 }
            });

        // Act
        using var command = Connection.CreateCommand();
        command.CommandText = "SELECT * FROM [Fridge] WHERE Amount > 0";
        using var reader = command.ExecuteReader();
        var actual1 = reader.FieldCount;
        actual1.Should().Be(0); // not initialized yet
        reader.Read();
        var actual2 = reader.FieldCount;
        actual2.Should().Be(2); // initialized because of Read
    }

    [Fact]
    public void Can_Stub_Multiple_Commands_For_ExecuteReader()
    {
        // Arrange
        // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
        Connection.AddResultForDataReader(command => command.CommandText == "SELECT * FROM [Fridge] WHERE Amount > 0", new[]
        {
                new MyRecord { Name = "Beer", Amount = 1 },
                new MyRecord { Name = "Milk", Amount = 3 }
            });
        Connection.AddResultForDataReader(command => command.CommandText == "SELECT * FROM [Fridge] WHERE Amount = 0", Array.Empty<MyRecord>());

        // Act
        using var commandWithResults = Connection.CreateCommand();
        commandWithResults.CommandText = "SELECT * FROM [Fridge] WHERE Amount > 0";
        using var readerWithResults = commandWithResults.ExecuteReader(CommandBehavior.SequentialAccess);
        var resultWithResults = new List<MyRecord>();
        while (readerWithResults.Read())
        {
            resultWithResults.Add(new MyRecord
            {
                Name = readerWithResults.GetString(readerWithResults.GetOrdinal("Name")),
                Amount = readerWithResults.GetInt32(readerWithResults.GetOrdinal("Amount"))
            });
        }

        using var commandWithoutResults = Connection.CreateCommand();
        commandWithoutResults.CommandText = "SELECT * FROM [Fridge] WHERE Amount = 0";
        using var readerWithoutResults = commandWithoutResults.ExecuteReader();
        var resultWithoutResults = new List<MyRecord>();
        while (readerWithoutResults.Read())
        {
            resultWithoutResults.Add(new MyRecord
            {
                Name = readerWithoutResults.GetString(readerWithoutResults.GetOrdinal("Name")),
                Amount = readerWithoutResults.GetInt32(readerWithoutResults.GetOrdinal("Amount"))
            });
        }

        // Assert
        resultWithResults.Should().HaveCount(2);
        resultWithResults[0].Name.Should().Be("Beer");
        resultWithResults[0].Amount.Should().Be(1);
        resultWithResults[1].Name.Should().Be("Milk");
        resultWithResults[1].Amount.Should().Be(3);
        resultWithoutResults.Should().HaveCount(0);
    }

    [Fact]
    public void Can_Interact_With_Command_By_Using_Parameters()
    {
        // Arrange
        // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
        int? parameterValue = null;
        Connection.AddResultForDataReader
        (
            command =>
            {
                var isCommandValid = command.CommandText == "SELECT * FROM [Fridge] WHERE Amount = @amount";
                if (isCommandValid)
                {
                    parameterValue = (int)command.Parameters.OfType<IDbDataParameter>().First().Value!;
                }
                return isCommandValid;
            },
            () => new[]
            {
                    new MyRecord { Name = "Beer", Amount = 1 },
                    new MyRecord { Name = "Milk", Amount = 3 }
            }.Where(x => x.Amount == parameterValue)
        );

        // Act
        using var commandWithResults = Connection.CreateCommand();
        commandWithResults.CommandText = "SELECT * FROM [Fridge] WHERE Amount = @amount";
        var parameter = commandWithResults.CreateParameter();
        parameter.ParameterName = "@amount";
        parameter.Value = 1;
        commandWithResults.Parameters.Add(parameter);
        using var readerWithResults = commandWithResults.ExecuteReader();
        var resultWithResults = new List<MyRecord>();
        while (readerWithResults.Read())
        {
            resultWithResults.Add(new MyRecord
            {
                Name = readerWithResults.GetString(readerWithResults.GetOrdinal("Name")),
                Amount = readerWithResults.GetInt32(readerWithResults.GetOrdinal("Amount"))
            });
        }

        // Assert
        resultWithResults.Should().HaveCount(1);
        resultWithResults[0].Name.Should().Be("Beer");
        resultWithResults[0].Amount.Should().Be(1);
    }

    [Fact]
    public void Can_Execute_Command_With_Multiple_ResultSets()
    {
        // Arrange
        // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
        Connection.AddResultForDataReader(reader => reader.NextResultCalled += (sender, args) =>
        {
            var secondReader = new DataReader(CommandBehavior.Default)
            {
                new MyRecord { Name = "Milk", Amount = 3 }
            };
            args.Dictionary = secondReader.Dictionary;
            args.CurrentIndex = secondReader.CurrentIndex + 1;
            args.Result = true;
        }, new[]
        {
                new MyRecord { Name = "Beer", Amount = 1 }
        });

        // Act
        using var command = Connection.CreateCommand();
        command.CommandText = "SELECT * FROM [Fridge] WHERE Amount > 0";
        using var reader = command.ExecuteReader();
        var result = new List<MyRecord>();
        while (reader.Read())
        {
            result.Add(new MyRecord
            {
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
            });
        }
        if (reader.NextResult())
        {
            result.Add(new MyRecord
            {
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
            });
        }

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Beer");
        result[0].Amount.Should().Be(1);
        result[1].Name.Should().Be("Milk");
        result[1].Amount.Should().Be(3);
    }

    [Fact]
    public void Can_Rollback_Transaction()
    {
        // Arrange
        using var transaction = Connection.BeginTransaction();
        var rolledBack = false;
        _callback.Transactions.Should().HaveCount(1);
        _callback.Transactions.First().RolledBack += (sender, args) => rolledBack = true;

        // Act
        transaction.Rollback();

        // Assert
        rolledBack.Should().BeTrue();
    }

    [Fact]
    public void Can_Commit_Transaction()
    {
        // Arrange
        using var transaction = Connection.BeginTransaction();
        var committed = false;
        _callback.Transactions.Should().HaveCount(1);
        _callback.Transactions.First().Committed += (sender, args) => committed = true;

        // Act
        transaction.Commit();

        // Assert
        committed.Should().BeTrue();
    }

    [Fact]
    public void Can_Add_Result_For_NonQueryCommand()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
        var actual = command.ExecuteNonQuery();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Can_Add_Result_For_NonQueryCommand_WithPredicate()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(cmd => cmd.CommandText.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase), 12345);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
        var actual = command.ExecuteNonQuery();

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    public void Can_Add_Result_For_NonQueryCommand_WithPredicate_AndD_ynamicDelegate()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(cmd => cmd.CommandText.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase), cmd => cmd.CommandText.Contains("Beer") ? 12345 : 0);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
        var actual = command.ExecuteNonQuery();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Can_Add_Result_For_NonQueryCommand_With_DynamicDelegate()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(cmd => cmd.CommandText.Contains("Beer") ? 12345 : 0);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
        var actual = command.ExecuteNonQuery();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Can_Add_Result_For_ScalarCommand()
    {
        // Arrange
        Connection.AddResultForScalarCommand(12345);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "SELECT COUNT(*) FROM MyTable";
        var actual = command.ExecuteScalar();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Can_Add_Result_For_ScalarCommand_With_Predicate()
    {
        // Arrange
        Connection.AddResultForScalarCommand(cmd => cmd.CommandText.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase), 12345);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "SELECT COUNT(*) FROM MyTable";
        var actual = command.ExecuteScalar();

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Can_Add_Result_For_ScalarCommand_With_Predicate_AndDynamic_Delegate()
    {
        // Arrange
        Connection.AddResultForScalarCommand(cmd => cmd.CommandText.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase), cmd => cmd.CommandText.Contains("MyTable") ? 12345 : 0);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "SELECT COUNT(*) FROM MyTable";
        var actual = command.ExecuteScalar();

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Can_Add_Result_For_ScalarCommand_With_DynamicDelegate()
    {
        // Arrange
        Connection.AddResultForScalarCommand(cmd => cmd.CommandText.Contains("MyTable") ? 12345 : 0);
        using var command = Connection.CreateCommand();

        // Act
        command.CommandText = "SELECT COUNT(*) FROM MyTable";
        var actual = command.ExecuteScalar();

        // Assert
        actual.Should().Be(12345);
    }

    private async Task ReaderTest(Func<IDataReader, MyRecord> recordDelegate)
    {
        // Arrange
        // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
        Connection.AddResultForDataReader(new[]
        {
            new MyRecord { Name = "Beer", Amount = 1 },
            new MyRecord { Name = "Milk", Amount = 3 }
        });

        // Act
        using var command = Connection.CreateCommand();
        command.CommandText = "SELECT * FROM [Fridge] WHERE Amount > 0";
        using var reader = await command.ExecuteReaderAsync();
        var result = new List<MyRecord>();
        while (await reader.ReadAsync())
        {
            result.Add(recordDelegate(reader));
        }

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Beer");
        result[0].Amount.Should().Be(1);
        result[1].Name.Should().Be("Milk");
        result[1].Amount.Should().Be(3);
    }

    public void Dispose() => Connection.Dispose();

    private sealed class MyRecord
    {
        public string? Name { get; set; }
        public int Amount { get; set; }
    }
}
