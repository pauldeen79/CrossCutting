using FluentAssertions;
using System.Collections.Generic;
using System.Data.Stub.Extensions;
using System.Linq;
using Xunit;

namespace System.Data.Stub.Tests
{
    public class IntegrationTests
    {
        private readonly DbConnection Connection;
        private readonly DbConnectionCallback _callback;

        public IntegrationTests()
        {
            _callback = new DbConnectionCallback();
            Connection = new DbConnection().AddCallback(_callback);
        }

        [Fact]
        public void CanStubCommandForExecuteNonQuery()
        {
            // Arrange
            using var command = Connection.CreateCommand();
            _callback.Commands.Last().ExecuteNonQueryResult = 12345;

            // Act
            command.CommandText = "INSERT INTO [Fridge](Name, Amount) VALUES ('Beer', 1)";
            var actual = command.ExecuteNonQuery();

            // Assert
            actual.Should().Be(12345);
        }

        [Fact]
        public void CanStubCommandForExecuteScalar()
        {
            // Arrange
            using var command = Connection.CreateCommand();
            _callback.Commands.Last().ExecuteScalarResult = 2;

            // Act
            command.CommandText = "SELCT MAX(Amount) FROM [Fridge]";
            var actual = command.ExecuteScalar();

            // Assert
            actual.Should().Be(2);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public void CanStubCommandForExecuteReader()
        {
            ReaderTest(reader => new MyRecord
            {
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
            });
        }

        [Fact]
        public void CanUseIndexerOnDataReader()
        {
            ReaderTest(reader => new MyRecord
            {
                Name = (string)reader["Name"],
                Amount = (int)reader["Amount"]
            });
        }

        [Fact]
        public void CanUseGetValueAndGetOrdinalOnDataReader()
        {
            ReaderTest(reader => new MyRecord
            {
                Name = (string)reader.GetValue(reader.GetOrdinal("Name")),
                Amount = (int)reader.GetValue(reader.GetOrdinal("Amount"))
            });
        }

        [Fact]
        public void CanUseGetValueAndColumnIndexOnDataReader()
        {
            ReaderTest(reader => new MyRecord
            {
                Name = (string)reader.GetValue(0),
                Amount = (int)reader.GetValue(1)
            });
        }
#pragma warning restore S2699 // Tests should include assertions

        [Fact]
        public void CanStubMultipleCommandsForExecuteReader()
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
        public void CanInteractWithCommandByUsingParameters()
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
                        parameterValue = (int)command.Parameters.OfType<IDbDataParameter>().First().Value;
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
        public void CanExecuteCommandWithMultipleResultSets()
        {
            // Arrange
            // Important to initialize datareader BEFORE creating the command! Else, the event won't fire
            Connection.AddResultForDataReader(reader => reader.NextResultCalled += (sender, args) =>
            {
                var secondReader = new DataReader(CommandBehavior.Default);
                secondReader.Add(new MyRecord { Name = "Milk", Amount = 3 });
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
        public void CanRollbackTransaction()
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
        public void CanCommitTransaction()
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

        private void ReaderTest(Func<IDataReader, MyRecord> recordDelegate)
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
            var result = new List<MyRecord>();
            while (reader.Read())
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

        private class MyRecord
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }
    }
}
