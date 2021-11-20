using System;
using System.Collections.Generic;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    public class TestEntityDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>
    {
        public Func<TestEntityBuilder, DatabaseOperation, IDatabaseCommand> CommandDelegate
            => (entity, operation) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return new StoredProcedureCommand<TestEntityBuilder>(@"[InsertEntity]", entity, DatabaseOperation.Insert, AddParameters);
                    case DatabaseOperation.Update:
                        return new StoredProcedureCommand<TestEntityBuilder>(@"[UpdateEntity]", entity, DatabaseOperation.Update, UpdateParameters);
                    case DatabaseOperation.Delete:
                        return new StoredProcedureCommand<TestEntityBuilder>(@"[DeleteEntity]", entity, DatabaseOperation.Delete, DeleteParameters);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");
                }
            };

        public Func<TestEntityBuilder, DatabaseOperation, TestEntityBuilder>? ResultEntityDelegate
            => (entity, operation) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return AddResultEntity(entity);
                    case DatabaseOperation.Update:
                        return UpdateResultEntity(entity);
                    case DatabaseOperation.Delete:
                        return DeleteResultEntity(entity);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");
                }
            };

        public Func<TestEntityBuilder, DatabaseOperation, IDataReader, TestEntityBuilder>? AfterReadDelegate
            => (entity, operation, reader) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return AddAfterRead(entity, reader);
                    case DatabaseOperation.Update:
                        return UpdateAfterRead(entity, reader);
                    case DatabaseOperation.Delete:
                        return entity;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");
                }
            };

        public Func<TestEntity, TestEntityBuilder>? CreateBuilderDelegate => entity => new TestEntityBuilder(entity);

        public Func<TestEntityBuilder, TestEntity>? CreateEntityDelegate => builder => builder.Build();

        private object AddParameters(TestEntityBuilder resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Code", resultEntity.Code),
                new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object?>("@Description", resultEntity.Description),
            };
        }

        private TestEntityBuilder AddResultEntity(TestEntityBuilder resultEntity)
        {

            return resultEntity;
        }

        private TestEntityBuilder AddAfterRead(TestEntityBuilder resultEntity, IDataReader reader)
        {
            // removed default(string) argument on required field
            resultEntity.Code = reader.GetString("Code");
            resultEntity.CodeType = reader.GetString("CodeType");
            resultEntity.Description = reader.GetString("Description");
            // Moved from AddFinalize!
            resultEntity.IsExistingEntity = true;
            return resultEntity;
        }

        private object UpdateParameters(TestEntityBuilder resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Code", resultEntity.Code),
                new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object?>("@Description", resultEntity.Description),
            };
        }

        private TestEntityBuilder UpdateResultEntity(TestEntityBuilder resultEntity)
        {

            return resultEntity;
        }

        private TestEntityBuilder UpdateAfterRead(TestEntityBuilder resultEntity, IDataReader reader)
        {
            // replace default(string) with string.Empty
            resultEntity.Code = reader.GetString("Code", string.Empty);
            resultEntity.CodeType = reader.GetString("CodeType", string.Empty);
            resultEntity.Description = reader.GetString("Description", string.Empty);

            return resultEntity;
        }

        private object DeleteParameters(TestEntityBuilder resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Code", resultEntity.Code),
                new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object?>("@Description", resultEntity.Description),
            };
        }

        private TestEntityBuilder DeleteResultEntity(TestEntityBuilder resultEntity)
        {

            return resultEntity;
        }
    }
}
