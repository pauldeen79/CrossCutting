using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRespositoryTests
    {
        [Fact]
        public void Can_Add_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", false);
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity("01", "Test", "first entity", true) });
            var callback = new DbConnectionCallback();
            connection.AddCallback(callback);
            var sut = new TestRepository(connection);

            // Act
            var result = sut.Save(input);

            // Assert
            result.IsExistingEntity.Should().BeTrue();
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("[InsertCode]");
        }

        [Fact]
        public void Can_Update_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity("01", "Test", "first entity", true) });
            var callback = new DbConnectionCallback();
            connection.AddCallback(callback);
            var sut = new TestRepository(connection);

            // Act
            var result = sut.Save(input);

            // Assert
            result.IsExistingEntity.Should().BeTrue();
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("[UpdateCode]");
        }

        [Fact]
        public void Can_Delete_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            var connection = new DbConnection();
            connection.AddResultForNonQueryCommand(1);
            var callback = new DbConnectionCallback();
            connection.AddCallback(callback);
            var sut = new TestRepository(connection);

            // Act
            var result = sut.Delete(input);

            // Assert
            result.IsExistingEntity.Should().BeTrue();
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("[DeleteCode]");
        }
    }
}
