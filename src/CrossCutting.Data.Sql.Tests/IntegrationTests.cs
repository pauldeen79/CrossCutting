﻿using System;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Tests.Repositories;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Sql.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class IntegrationTests : IDisposable
    {
        private readonly TestRepository _repository;
        private readonly IDatabaseEntityMapper<TestEntity> _mapper;
        private readonly DbConnection _connection;

        public IntegrationTests()
        {
            _connection = new DbConnection();
            _mapper = new TestEntityMapper();
            _repository = new TestRepository
                (
                    new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(_connection, new TestEntityDatabaseCommandEntityProvider()),
                    new DatabaseEntityRetriever<TestEntity>(_connection, _mapper),
                    new TestEntityIdentityDatabaseCommandProvider(),
                    new TestEntityDatabaseCommandProvider()
                );
        }

        [Fact]
        public void Can_Add_Entity()
        {
            // Arrange
            var entity = new TestEntity("A", "B", "C", false);
            _connection.AddResultForDataReader(cmd => cmd.CommandText == "INSERT INTO...", new[] { new TestEntity("A", "B", "C", true) });

            // Act
            var actual =_repository.Add(entity);

            // Assert
            actual.Code.Should().Be(entity.Code);
            actual.CodeType.Should().Be(entity.CodeType);
            actual.Description.Should().Be(entity.Description);
            actual.IsExistingEntity.Should().BeTrue();
        }

        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            var entity = new TestEntity("A", "B", "C", true);
            _connection.AddResultForDataReader(cmd => cmd.CommandText == "UPDATE...", new[] { new TestEntity("A1", "B1", "C1", true) });

            // Act
            var actual = _repository.Update(entity);

            // Assert
            actual.Code.Should().Be(entity.Code + "1");
            actual.CodeType.Should().Be(entity.CodeType + "1");
            actual.Description.Should().Be(entity.Description + "1");
            actual.IsExistingEntity.Should().BeTrue();
        }

        [Fact]
        public void Can_Delete_Entity()
        {
            // Arrange
            var entity = new TestEntity("A", "B", "C", true);
            _connection.AddResultForDataReader(cmd => cmd.CommandText == "DELETE...", new[] { new TestEntity("A1", "B1", "C1", true) }); //suffixes get ignored because Delete does not read result

            // Act
            var actual = _repository.Delete(entity);

            // Assert
            actual.Code.Should().Be(entity.Code);
            actual.CodeType.Should().Be(entity.CodeType);
            actual.Description.Should().Be(entity.Description);
            actual.IsExistingEntity.Should().BeTrue();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}