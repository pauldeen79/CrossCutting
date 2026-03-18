using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseCommandProviders
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogCommandProvider : IDatabaseCommandProvider<Catalog>
    {
        public Task<Result<IDatabaseCommand>> CreateAsync(Catalog source, DatabaseOperation operation, CancellationToken token)
        {
            return  Task.Run(() =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return Result.Success<IDatabaseCommand>(new StoredProcedureCommand<Catalog>(@"[InsertCatalog]", source, DatabaseOperation.Insert, AddParameters));
                    case DatabaseOperation.Update:
                        return Result.Success<IDatabaseCommand>(new StoredProcedureCommand<Catalog>(@"[UpdateCatalog]", source, DatabaseOperation.Update, UpdateParameters));
                    case DatabaseOperation.Delete:
                        return Result.Success<IDatabaseCommand>(new StoredProcedureCommand<Catalog>(@"[DeleteCatalog]", source, DatabaseOperation.Delete, DeleteParameters));
                    default:
                        return Result.Invalid<IDatabaseCommand>(string.Format("Unsupported operation: {0}", operation));
                }
            }, token);
        }

        private object AddParameters(Catalog resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Name", resultEntity.Name),
                new KeyValuePair<string, object?>("@DateCreated", resultEntity.DateCreated),
                new KeyValuePair<string, object?>("@DateLastModified", resultEntity.DateLastModified),
                new KeyValuePair<string, object?>("@DateSynchronized", resultEntity.DateSynchronized),
                new KeyValuePair<string, object?>("@DriveSerialNumber", resultEntity.DriveSerialNumber),
                new KeyValuePair<string, object?>("@DriveTypeCodeType", resultEntity.DriveTypeCodeType),
                new KeyValuePair<string, object?>("@DriveTypeCode", resultEntity.DriveTypeCode),
                new KeyValuePair<string, object?>("@DriveTotalSize", resultEntity.DriveTotalSize),
                new KeyValuePair<string, object?>("@DriveFreeSpace", resultEntity.DriveFreeSpace),
                new KeyValuePair<string, object?>("@Recursive", resultEntity.Recursive),
                new KeyValuePair<string, object?>("@Sorted", resultEntity.Sorted),
                new KeyValuePair<string, object?>("@StartDirectory", resultEntity.StartDirectory),
                new KeyValuePair<string, object?>("@ExtraField1", resultEntity.ExtraField1),
                new KeyValuePair<string, object?>("@ExtraField2", resultEntity.ExtraField2),
                new KeyValuePair<string, object?>("@ExtraField3", resultEntity.ExtraField3),
                new KeyValuePair<string, object?>("@ExtraField4", resultEntity.ExtraField4),
                new KeyValuePair<string, object?>("@ExtraField5", resultEntity.ExtraField5),
                new KeyValuePair<string, object?>("@ExtraField6", resultEntity.ExtraField6),
                new KeyValuePair<string, object?>("@ExtraField7", resultEntity.ExtraField7),
                new KeyValuePair<string, object?>("@ExtraField8", resultEntity.ExtraField8),
                new KeyValuePair<string, object?>("@ExtraField9", resultEntity.ExtraField9),
                new KeyValuePair<string, object?>("@ExtraField10", resultEntity.ExtraField10),
                new KeyValuePair<string, object?>("@ExtraField11", resultEntity.ExtraField11),
                new KeyValuePair<string, object?>("@ExtraField12", resultEntity.ExtraField12),
                new KeyValuePair<string, object?>("@ExtraField13", resultEntity.ExtraField13),
                new KeyValuePair<string, object?>("@ExtraField14", resultEntity.ExtraField14),
                new KeyValuePair<string, object?>("@ExtraField15", resultEntity.ExtraField15),
                new KeyValuePair<string, object?>("@ExtraField16", resultEntity.ExtraField16),
            };
        }

        private object UpdateParameters(Catalog resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Name", resultEntity.Name),
                new KeyValuePair<string, object?>("@DateCreated", resultEntity.DateCreated),
                new KeyValuePair<string, object?>("@DateLastModified", resultEntity.DateLastModified),
                new KeyValuePair<string, object?>("@DateSynchronized", resultEntity.DateSynchronized),
                new KeyValuePair<string, object?>("@DriveSerialNumber", resultEntity.DriveSerialNumber),
                new KeyValuePair<string, object?>("@DriveTypeCodeType", resultEntity.DriveTypeCodeType),
                new KeyValuePair<string, object?>("@DriveTypeCode", resultEntity.DriveTypeCode),
                new KeyValuePair<string, object?>("@DriveTotalSize", resultEntity.DriveTotalSize),
                new KeyValuePair<string, object?>("@DriveFreeSpace", resultEntity.DriveFreeSpace),
                new KeyValuePair<string, object?>("@Recursive", resultEntity.Recursive),
                new KeyValuePair<string, object?>("@Sorted", resultEntity.Sorted),
                new KeyValuePair<string, object?>("@StartDirectory", resultEntity.StartDirectory),
                new KeyValuePair<string, object?>("@ExtraField1", resultEntity.ExtraField1),
                new KeyValuePair<string, object?>("@ExtraField2", resultEntity.ExtraField2),
                new KeyValuePair<string, object?>("@ExtraField3", resultEntity.ExtraField3),
                new KeyValuePair<string, object?>("@ExtraField4", resultEntity.ExtraField4),
                new KeyValuePair<string, object?>("@ExtraField5", resultEntity.ExtraField5),
                new KeyValuePair<string, object?>("@ExtraField6", resultEntity.ExtraField6),
                new KeyValuePair<string, object?>("@ExtraField7", resultEntity.ExtraField7),
                new KeyValuePair<string, object?>("@ExtraField8", resultEntity.ExtraField8),
                new KeyValuePair<string, object?>("@ExtraField9", resultEntity.ExtraField9),
                new KeyValuePair<string, object?>("@ExtraField10", resultEntity.ExtraField10),
                new KeyValuePair<string, object?>("@ExtraField11", resultEntity.ExtraField11),
                new KeyValuePair<string, object?>("@ExtraField12", resultEntity.ExtraField12),
                new KeyValuePair<string, object?>("@ExtraField13", resultEntity.ExtraField13),
                new KeyValuePair<string, object?>("@ExtraField14", resultEntity.ExtraField14),
                new KeyValuePair<string, object?>("@ExtraField15", resultEntity.ExtraField15),
                new KeyValuePair<string, object?>("@ExtraField16", resultEntity.ExtraField16),
                new KeyValuePair<string, object?>("@IdOriginal", resultEntity.IdOriginal),
                new KeyValuePair<string, object?>("@NameOriginal", resultEntity.NameOriginal),
                new KeyValuePair<string, object?>("@DateCreatedOriginal", resultEntity.DateCreatedOriginal),
                new KeyValuePair<string, object?>("@DateLastModifiedOriginal", resultEntity.DateLastModifiedOriginal),
                new KeyValuePair<string, object?>("@DateSynchronizedOriginal", resultEntity.DateSynchronizedOriginal),
                new KeyValuePair<string, object?>("@DriveSerialNumberOriginal", resultEntity.DriveSerialNumberOriginal),
                new KeyValuePair<string, object?>("@DriveTypeCodeTypeOriginal", resultEntity.DriveTypeCodeTypeOriginal),
                new KeyValuePair<string, object?>("@DriveTypeCodeOriginal", resultEntity.DriveTypeCodeOriginal),
                new KeyValuePair<string, object?>("@DriveTotalSizeOriginal", resultEntity.DriveTotalSizeOriginal),
                new KeyValuePair<string, object?>("@DriveFreeSpaceOriginal", resultEntity.DriveFreeSpaceOriginal),
                new KeyValuePair<string, object?>("@RecursiveOriginal", resultEntity.RecursiveOriginal),
                new KeyValuePair<string, object?>("@SortedOriginal", resultEntity.SortedOriginal),
                new KeyValuePair<string, object?>("@StartDirectoryOriginal", resultEntity.StartDirectoryOriginal),
                new KeyValuePair<string, object?>("@ExtraField1Original", resultEntity.ExtraField1Original),
                new KeyValuePair<string, object?>("@ExtraField2Original", resultEntity.ExtraField2Original),
                new KeyValuePair<string, object?>("@ExtraField3Original", resultEntity.ExtraField3Original),
                new KeyValuePair<string, object?>("@ExtraField4Original", resultEntity.ExtraField4Original),
                new KeyValuePair<string, object?>("@ExtraField5Original", resultEntity.ExtraField5Original),
                new KeyValuePair<string, object?>("@ExtraField6Original", resultEntity.ExtraField6Original),
                new KeyValuePair<string, object?>("@ExtraField7Original", resultEntity.ExtraField7Original),
                new KeyValuePair<string, object?>("@ExtraField8Original", resultEntity.ExtraField8Original),
                new KeyValuePair<string, object?>("@ExtraField9Original", resultEntity.ExtraField9Original),
                new KeyValuePair<string, object?>("@ExtraField10Original", resultEntity.ExtraField10Original),
                new KeyValuePair<string, object?>("@ExtraField11Original", resultEntity.ExtraField11Original),
                new KeyValuePair<string, object?>("@ExtraField12Original", resultEntity.ExtraField12Original),
                new KeyValuePair<string, object?>("@ExtraField13Original", resultEntity.ExtraField13Original),
                new KeyValuePair<string, object?>("@ExtraField14Original", resultEntity.ExtraField14Original),
                new KeyValuePair<string, object?>("@ExtraField15Original", resultEntity.ExtraField15Original),
                new KeyValuePair<string, object?>("@ExtraField16Original", resultEntity.ExtraField16Original),
            };
        }

        private object DeleteParameters(Catalog resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object?>("@Id", resultEntity.Id),
                new KeyValuePair<string, object?>("@Name", resultEntity.Name),
                new KeyValuePair<string, object?>("@DateCreated", resultEntity.DateCreated),
                new KeyValuePair<string, object?>("@DateLastModified", resultEntity.DateLastModified),
                new KeyValuePair<string, object?>("@DateSynchronized", resultEntity.DateSynchronized),
                new KeyValuePair<string, object?>("@DriveSerialNumber", resultEntity.DriveSerialNumber),
                new KeyValuePair<string, object?>("@DriveTypeCodeType", resultEntity.DriveTypeCodeType),
                new KeyValuePair<string, object?>("@DriveTypeCode", resultEntity.DriveTypeCode),
                new KeyValuePair<string, object?>("@DriveTotalSize", resultEntity.DriveTotalSize),
                new KeyValuePair<string, object?>("@DriveFreeSpace", resultEntity.DriveFreeSpace),
                new KeyValuePair<string, object?>("@Recursive", resultEntity.Recursive),
                new KeyValuePair<string, object?>("@Sorted", resultEntity.Sorted),
                new KeyValuePair<string, object?>("@StartDirectory", resultEntity.StartDirectory),
                new KeyValuePair<string, object?>("@ExtraField1", resultEntity.ExtraField1),
                new KeyValuePair<string, object?>("@ExtraField2", resultEntity.ExtraField2),
                new KeyValuePair<string, object?>("@ExtraField3", resultEntity.ExtraField3),
                new KeyValuePair<string, object?>("@ExtraField4", resultEntity.ExtraField4),
                new KeyValuePair<string, object?>("@ExtraField5", resultEntity.ExtraField5),
                new KeyValuePair<string, object?>("@ExtraField6", resultEntity.ExtraField6),
                new KeyValuePair<string, object?>("@ExtraField7", resultEntity.ExtraField7),
                new KeyValuePair<string, object?>("@ExtraField8", resultEntity.ExtraField8),
                new KeyValuePair<string, object?>("@ExtraField9", resultEntity.ExtraField9),
                new KeyValuePair<string, object?>("@ExtraField10", resultEntity.ExtraField10),
                new KeyValuePair<string, object?>("@ExtraField11", resultEntity.ExtraField11),
                new KeyValuePair<string, object?>("@ExtraField12", resultEntity.ExtraField12),
                new KeyValuePair<string, object?>("@ExtraField13", resultEntity.ExtraField13),
                new KeyValuePair<string, object?>("@ExtraField14", resultEntity.ExtraField14),
                new KeyValuePair<string, object?>("@ExtraField15", resultEntity.ExtraField15),
                new KeyValuePair<string, object?>("@ExtraField16", resultEntity.ExtraField16),
            };
        }
    }
#nullable restore
}
