using System;
using System.Collections.Generic;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    public class TestEntityDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity>
    {
        public Func<TestEntity, DatabaseOperation, IDatabaseCommand> CommandDelegate
            => (entity, operation) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return new StoredProcedureCommand<TestEntity>(@"[InsertEntity]", entity, DatabaseOperation.Insert, AddParameters);
                    case DatabaseOperation.Update:
                        return new StoredProcedureCommand<TestEntity>(@"[UpdateEntity]", entity, DatabaseOperation.Update, UpdateParameters);
                    case DatabaseOperation.Delete:
                        return new StoredProcedureCommand<TestEntity>(@"[DeleteEntity]", entity, DatabaseOperation.Delete, DeleteParameters);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");
                }
            };

        public Func<TestEntity, DatabaseOperation, TestEntity>? ResultEntityDelegate
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

        public Func<TestEntity, DatabaseOperation, IDataReader, TestEntity>? AfterReadDelegate
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

        private object AddParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity AddResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity AddAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            // replace default(string) with string.Empty
            resultEntity.Code = reader.GetString("Code", string.Empty);
            resultEntity.CodeType = reader.GetString("CodeType", string.Empty);
            resultEntity.Description = reader.GetString("Description", string.Empty);
            // Moved from AddFinalize!
            resultEntity.IsExistingEntity = true;
            return resultEntity;
        }

        private object UpdateParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity UpdateResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity UpdateAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            // replace default(string) with string.Empty
            resultEntity.Code = reader.GetString("Code", string.Empty);
            resultEntity.CodeType = reader.GetString("CodeType", string.Empty);
            resultEntity.Description = reader.GetString("Description", string.Empty);

            return resultEntity;
        }

        private object DeleteParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity DeleteResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }
    }
}
