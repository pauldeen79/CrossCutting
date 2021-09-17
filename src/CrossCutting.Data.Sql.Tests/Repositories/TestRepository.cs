using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository
    {
        public TestEntity Add(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new StoredProcedureCommand<TestEntity>(@"[InsertCode]", x, AddParameters),
                typeof(TestEntity).Name + " entity was not added",
                AddResultEntity,
                AddAfterRead,
                AddFinalize
            );
        }

        private TestEntity AddResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity AddFinalize(TestEntity resultEntity, Exception? exception)
        {
            resultEntity.IsExistingEntity = true;
            return resultEntity;
        }

        private TestEntity AddAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            resultEntity.Code =reader.GetString("Code", default(string));
            resultEntity.CodeType = reader.GetString("CodeType", default(string));
            resultEntity.Description = reader.GetString("Description", default(string));

            return resultEntity;
        }

        private object AddParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        public TestEntity Update(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new StoredProcedureCommand<TestEntity>(@"[UpdateCode]", x, UpdateParameters),
                typeof(TestEntity).Name + " entity was not updated",
                UpdateResultEntity,
                UpdateAfterRead
            );
        }

        private TestEntity UpdateResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity UpdateAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            resultEntity.Code = reader.GetString("Code", default(string));
            resultEntity.CodeType = reader.GetString("CodeType", default(string));
            resultEntity.Description = reader.GetString("Description", default(string));

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

        public TestEntity Delete(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new StoredProcedureCommand<TestEntity>(@"[DeleteCode]", x, DeleteParameters),
                typeof(TestEntity).Name + " entity was not deleted",
                DeleteResultEntity
            );
        }

        private TestEntity DeleteResultEntity(TestEntity resultEntity)
        {

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

        public TestRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connection = connection;

        }

        private readonly IDbConnection _connection;
    }

    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public bool IsExistingEntity { get; set; }
        public string Code { get; set; }
        public string CodeType { get; set; }
        public string Description { get; set; }

        public TestEntity(string code, string codeType, string description, bool isExistingEntity = false)
        {
            Code = code;
            CodeType = codeType;
            Description = description;
            IsExistingEntity = isExistingEntity;
        }
    }
}
