using System;
using System.CodeDom.Compiler;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseCommandEntityProviders
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<Catalog, CatalogBuilder>
    {
        public CreateResultEntityHandler<CatalogBuilder>? CreateResultEntity
        {
            get
            {
                return (entity, operation) =>
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
                            throw new ArgumentOutOfRangeException(nameof(operation), string.Format("Unsupported operation: {0}", operation));
                    }
                };
            }
        }

        public AfterReadHandler<CatalogBuilder>? AfterRead
        {
            get
            {
                return (entity, operation, reader) =>
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
                            throw new ArgumentOutOfRangeException(nameof(operation), string.Format("Unsupported operation: {0}", operation));
                    }
                };
            }
        }

        public CreateBuilderHandler<Catalog, CatalogBuilder>? CreateBuilder
        {
            get
            {
                return entity => new CatalogBuilder(entity);
            }
        }

        public CreateEntityHandler<CatalogBuilder, Catalog>? CreateEntity
        {
            get
            {
                return builder => builder.Build();
            }
        }

        private CatalogBuilder AddResultEntity(CatalogBuilder resultEntity)
        {
            // Moved from Finalize
            resultEntity = resultEntity.WithIsExistingEntity(true);
            return resultEntity;
        }

        private CatalogBuilder AddAfterRead(CatalogBuilder resultEntity, IDataReader reader)
        {
            resultEntity = resultEntity.WithId(reader.GetInt32("Id"));
            resultEntity = resultEntity.WithName(reader.GetString("Name"));
            resultEntity = resultEntity.WithDateCreated(reader.GetDateTime("DateCreated"));
            resultEntity = resultEntity.WithDateLastModified(reader.GetDateTime("DateLastModified"));
            resultEntity = resultEntity.WithDateSynchronized(reader.GetDateTime("DateSynchronized"));
            resultEntity = resultEntity.WithDriveSerialNumber(reader.GetString("DriveSerialNumber"));
            resultEntity = resultEntity.WithDriveTypeCodeType(reader.GetString("DriveTypeCodeType"));
            resultEntity = resultEntity.WithDriveTypeCode(reader.GetString("DriveTypeCode"));
            resultEntity = resultEntity.WithDriveTotalSize(reader.GetInt32("DriveTotalSize"));
            resultEntity = resultEntity.WithDriveFreeSpace(reader.GetInt32("DriveFreeSpace"));
            resultEntity = resultEntity.WithRecursive(reader.GetBoolean("Recursive"));
            resultEntity = resultEntity.WithSorted(reader.GetBoolean("Sorted"));
            resultEntity = resultEntity.WithStartDirectory(reader.GetString("StartDirectory"));
            resultEntity = resultEntity.WithExtraField1(reader.GetNullableString("ExtraField1"));
            resultEntity = resultEntity.WithExtraField2(reader.GetNullableString("ExtraField2"));
            resultEntity = resultEntity.WithExtraField3(reader.GetNullableString("ExtraField3"));
            resultEntity = resultEntity.WithExtraField4(reader.GetNullableString("ExtraField4"));
            resultEntity = resultEntity.WithExtraField5(reader.GetNullableString("ExtraField5"));
            resultEntity = resultEntity.WithExtraField6(reader.GetNullableString("ExtraField6"));
            resultEntity = resultEntity.WithExtraField7(reader.GetNullableString("ExtraField7"));
            resultEntity = resultEntity.WithExtraField8(reader.GetNullableString("ExtraField8"));
            resultEntity = resultEntity.WithExtraField9(reader.GetNullableString("ExtraField9"));
            resultEntity = resultEntity.WithExtraField10(reader.GetNullableString("ExtraField10"));
            resultEntity = resultEntity.WithExtraField11(reader.GetNullableString("ExtraField11"));
            resultEntity = resultEntity.WithExtraField12(reader.GetNullableString("ExtraField12"));
            resultEntity = resultEntity.WithExtraField13(reader.GetNullableString("ExtraField13"));
            resultEntity = resultEntity.WithExtraField14(reader.GetNullableString("ExtraField14"));
            resultEntity = resultEntity.WithExtraField15(reader.GetNullableString("ExtraField15"));
            resultEntity = resultEntity.WithExtraField16(reader.GetNullableString("ExtraField16"));
            resultEntity = resultEntity.WithIdOriginal(reader.GetInt32("Id"));
            resultEntity = resultEntity.WithNameOriginal(reader.GetString("Name"));
            resultEntity = resultEntity.WithDateCreatedOriginal(reader.GetDateTime("DateCreated"));
            resultEntity = resultEntity.WithDateLastModifiedOriginal(reader.GetDateTime("DateLastModified"));
            resultEntity = resultEntity.WithDateSynchronizedOriginal(reader.GetDateTime("DateSynchronized"));
            resultEntity = resultEntity.WithDriveSerialNumberOriginal(reader.GetString("DriveSerialNumber"));
            resultEntity = resultEntity.WithDriveTypeCodeTypeOriginal(reader.GetString("DriveTypeCodeType"));
            resultEntity = resultEntity.WithDriveTypeCodeOriginal(reader.GetString("DriveTypeCode"));
            resultEntity = resultEntity.WithDriveTotalSizeOriginal(reader.GetInt32("DriveTotalSize"));
            resultEntity = resultEntity.WithDriveFreeSpaceOriginal(reader.GetInt32("DriveFreeSpace"));
            resultEntity = resultEntity.WithRecursiveOriginal(reader.GetBoolean("Recursive"));
            resultEntity = resultEntity.WithSortedOriginal(reader.GetBoolean("Sorted"));
            resultEntity = resultEntity.WithStartDirectoryOriginal(reader.GetString("StartDirectory"));
            resultEntity = resultEntity.WithExtraField1Original(reader.GetNullableString("ExtraField1"));
            resultEntity = resultEntity.WithExtraField2Original(reader.GetNullableString("ExtraField2"));
            resultEntity = resultEntity.WithExtraField3Original(reader.GetNullableString("ExtraField3"));
            resultEntity = resultEntity.WithExtraField4Original(reader.GetNullableString("ExtraField4"));
            resultEntity = resultEntity.WithExtraField5Original(reader.GetNullableString("ExtraField5"));
            resultEntity = resultEntity.WithExtraField6Original(reader.GetNullableString("ExtraField6"));
            resultEntity = resultEntity.WithExtraField7Original(reader.GetNullableString("ExtraField7"));
            resultEntity = resultEntity.WithExtraField8Original(reader.GetNullableString("ExtraField8"));
            resultEntity = resultEntity.WithExtraField9Original(reader.GetNullableString("ExtraField9"));
            resultEntity = resultEntity.WithExtraField10Original(reader.GetNullableString("ExtraField10"));
            resultEntity = resultEntity.WithExtraField11Original(reader.GetNullableString("ExtraField11"));
            resultEntity = resultEntity.WithExtraField12Original(reader.GetNullableString("ExtraField12"));
            resultEntity = resultEntity.WithExtraField13Original(reader.GetNullableString("ExtraField13"));
            resultEntity = resultEntity.WithExtraField14Original(reader.GetNullableString("ExtraField14"));
            resultEntity = resultEntity.WithExtraField15Original(reader.GetNullableString("ExtraField15"));
            resultEntity = resultEntity.WithExtraField16Original(reader.GetNullableString("ExtraField16"));

            return resultEntity;
        }

        private CatalogBuilder UpdateResultEntity(CatalogBuilder resultEntity)
        {

            return resultEntity;
        }

        private CatalogBuilder UpdateAfterRead(CatalogBuilder resultEntity, IDataReader reader)
        {
            resultEntity = resultEntity.WithId(reader.GetInt32("Id"));
            resultEntity = resultEntity.WithName(reader.GetString("Name"));
            resultEntity = resultEntity.WithDateCreated(reader.GetDateTime("DateCreated"));
            resultEntity = resultEntity.WithDateLastModified(reader.GetDateTime("DateLastModified"));
            resultEntity = resultEntity.WithDateSynchronized(reader.GetDateTime("DateSynchronized"));
            resultEntity = resultEntity.WithDriveSerialNumber(reader.GetString("DriveSerialNumber"));
            resultEntity = resultEntity.WithDriveTypeCodeType(reader.GetString("DriveTypeCodeType"));
            resultEntity = resultEntity.WithDriveTypeCode(reader.GetString("DriveTypeCode"));
            resultEntity = resultEntity.WithDriveTotalSize(reader.GetInt32("DriveTotalSize"));
            resultEntity = resultEntity.WithDriveFreeSpace(reader.GetInt32("DriveFreeSpace"));
            resultEntity = resultEntity.WithRecursive(reader.GetBoolean("Recursive"));
            resultEntity = resultEntity.WithSorted(reader.GetBoolean("Sorted"));
            resultEntity = resultEntity.WithStartDirectory(reader.GetString("StartDirectory"));
            resultEntity = resultEntity.WithExtraField1(reader.GetNullableString("ExtraField1"));
            resultEntity = resultEntity.WithExtraField2(reader.GetNullableString("ExtraField2"));
            resultEntity = resultEntity.WithExtraField3(reader.GetNullableString("ExtraField3"));
            resultEntity = resultEntity.WithExtraField4(reader.GetNullableString("ExtraField4"));
            resultEntity = resultEntity.WithExtraField5(reader.GetNullableString("ExtraField5"));
            resultEntity = resultEntity.WithExtraField6(reader.GetNullableString("ExtraField6"));
            resultEntity = resultEntity.WithExtraField7(reader.GetNullableString("ExtraField7"));
            resultEntity = resultEntity.WithExtraField8(reader.GetNullableString("ExtraField8"));
            resultEntity = resultEntity.WithExtraField9(reader.GetNullableString("ExtraField9"));
            resultEntity = resultEntity.WithExtraField10(reader.GetNullableString("ExtraField10"));
            resultEntity = resultEntity.WithExtraField11(reader.GetNullableString("ExtraField11"));
            resultEntity = resultEntity.WithExtraField12(reader.GetNullableString("ExtraField12"));
            resultEntity = resultEntity.WithExtraField13(reader.GetNullableString("ExtraField13"));
            resultEntity = resultEntity.WithExtraField14(reader.GetNullableString("ExtraField14"));
            resultEntity = resultEntity.WithExtraField15(reader.GetNullableString("ExtraField15"));
            resultEntity = resultEntity.WithExtraField16(reader.GetNullableString("ExtraField16"));
            resultEntity = resultEntity.WithIdOriginal(reader.GetInt32("Id"));
            resultEntity = resultEntity.WithNameOriginal(reader.GetString("Name"));
            resultEntity = resultEntity.WithDateCreatedOriginal(reader.GetDateTime("DateCreated"));
            resultEntity = resultEntity.WithDateLastModifiedOriginal(reader.GetDateTime("DateLastModified"));
            resultEntity = resultEntity.WithDateSynchronizedOriginal(reader.GetDateTime("DateSynchronized"));
            resultEntity = resultEntity.WithDriveSerialNumberOriginal(reader.GetString("DriveSerialNumber"));
            resultEntity = resultEntity.WithDriveTypeCodeTypeOriginal(reader.GetString("DriveTypeCodeType"));
            resultEntity = resultEntity.WithDriveTypeCodeOriginal(reader.GetString("DriveTypeCode"));
            resultEntity = resultEntity.WithDriveTotalSizeOriginal(reader.GetInt32("DriveTotalSize"));
            resultEntity = resultEntity.WithDriveFreeSpaceOriginal(reader.GetInt32("DriveFreeSpace"));
            resultEntity = resultEntity.WithRecursiveOriginal(reader.GetBoolean("Recursive"));
            resultEntity = resultEntity.WithSortedOriginal(reader.GetBoolean("Sorted"));
            resultEntity = resultEntity.WithStartDirectoryOriginal(reader.GetString("StartDirectory"));
            resultEntity = resultEntity.WithExtraField1Original(reader.GetNullableString("ExtraField1"));
            resultEntity = resultEntity.WithExtraField2Original(reader.GetNullableString("ExtraField2"));
            resultEntity = resultEntity.WithExtraField3Original(reader.GetNullableString("ExtraField3"));
            resultEntity = resultEntity.WithExtraField4Original(reader.GetNullableString("ExtraField4"));
            resultEntity = resultEntity.WithExtraField5Original(reader.GetNullableString("ExtraField5"));
            resultEntity = resultEntity.WithExtraField6Original(reader.GetNullableString("ExtraField6"));
            resultEntity = resultEntity.WithExtraField7Original(reader.GetNullableString("ExtraField7"));
            resultEntity = resultEntity.WithExtraField8Original(reader.GetNullableString("ExtraField8"));
            resultEntity = resultEntity.WithExtraField9Original(reader.GetNullableString("ExtraField9"));
            resultEntity = resultEntity.WithExtraField10Original(reader.GetNullableString("ExtraField10"));
            resultEntity = resultEntity.WithExtraField11Original(reader.GetNullableString("ExtraField11"));
            resultEntity = resultEntity.WithExtraField12Original(reader.GetNullableString("ExtraField12"));
            resultEntity = resultEntity.WithExtraField13Original(reader.GetNullableString("ExtraField13"));
            resultEntity = resultEntity.WithExtraField14Original(reader.GetNullableString("ExtraField14"));
            resultEntity = resultEntity.WithExtraField15Original(reader.GetNullableString("ExtraField15"));
            resultEntity = resultEntity.WithExtraField16Original(reader.GetNullableString("ExtraField16"));

            return resultEntity;
        }

        private CatalogBuilder DeleteResultEntity(CatalogBuilder resultEntity)
        {

            return resultEntity;
        }
    }
#nullable restore
}
