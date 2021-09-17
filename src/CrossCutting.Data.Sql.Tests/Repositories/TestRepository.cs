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
        private TestEntity Add(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.Add
            (
                instance,
                IsAdd,
                x => new StoredProcedureCommand<TestEntity>(@"[InsertCode]", x, AddParameters),
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

        private TestEntity Update(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.Update
            (
                instance,
                IsAdd,
                x => new StoredProcedureCommand<TestEntity>(@"[UpdateCode]", x, UpdateParameters),
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
            return _connection.Delete
            (
                instance,
                IsAdd,
                x => new StoredProcedureCommand<TestEntity>(@"[DeleteCode]", x, DeleteParameters),
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

        private bool IsAdd(TestEntity instance)
        {
            return !instance.IsExistingEntity;
        }

        public TestEntity Save(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (IsAdd(instance))
            {
                return Add(instance);
            }
            else
            {
                return Update(instance);
            }
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
