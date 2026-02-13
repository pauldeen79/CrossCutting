using System.CodeDom.Compiler;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.EntityMappers
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogEntityMapper : IDatabaseEntityMapper<Catalog>
    {
        public Catalog Map(IDataReader reader)
        {
            return new Catalog
            (
                id: reader.GetInt32("Id"),
                name: reader.GetString("Name"),
                dateCreated: reader.GetDateTime("DateCreated"),
                dateLastModified: reader.GetDateTime("DateLastModified"),
                dateSynchronized: reader.GetDateTime("DateSynchronized"),
                driveSerialNumber: reader.GetString("DriveSerialNumber"),
                driveTypeCodeType: reader.GetString("DriveTypeCodeType"),
                driveTypeCode: reader.GetString("DriveTypeCode"),
                driveTypeDescription: reader.GetNullableString("DriveTypeDescription"),
                driveTotalSize: reader.GetInt32("DriveTotalSize"),
                driveFreeSpace: reader.GetInt32("DriveFreeSpace"),
                recursive: reader.GetBoolean("Recursive"),
                sorted: reader.GetBoolean("Sorted"),
                startDirectory: reader.GetString("StartDirectory"),
                extraField1: reader.GetNullableString("ExtraField1"),
                extraField2: reader.GetNullableString("ExtraField2"),
                extraField3: reader.GetNullableString("ExtraField3"),
                extraField4: reader.GetNullableString("ExtraField4"),
                extraField5: reader.GetNullableString("ExtraField5"),
                extraField6: reader.GetNullableString("ExtraField6"),
                extraField7: reader.GetNullableString("ExtraField7"),
                extraField8: reader.GetNullableString("ExtraField8"),
                extraField9: reader.GetNullableString("ExtraField9"),
                extraField10: reader.GetNullableString("ExtraField10"),
                extraField11: reader.GetNullableString("ExtraField11"),
                extraField12: reader.GetNullableString("ExtraField12"),
                extraField13: reader.GetNullableString("ExtraField13"),
                extraField14: reader.GetNullableString("ExtraField14"),
                extraField15: reader.GetNullableString("ExtraField15"),
                extraField16: reader.GetNullableString("ExtraField16"),
                isExistingEntity: true,
                idOriginal: reader.GetInt32("Id"),
                nameOriginal: reader.GetString("Name"),
                dateCreatedOriginal: reader.GetDateTime("DateCreated"),
                dateLastModifiedOriginal: reader.GetDateTime("DateLastModified"),
                dateSynchronizedOriginal: reader.GetDateTime("DateSynchronized"),
                driveSerialNumberOriginal: reader.GetString("DriveSerialNumber"),
                driveTypeCodeTypeOriginal: reader.GetString("DriveTypeCodeType"),
                driveTypeCodeOriginal: reader.GetString("DriveTypeCode"),
                driveTotalSizeOriginal: reader.GetInt32("DriveTotalSize"),
                driveFreeSpaceOriginal: reader.GetInt32("DriveFreeSpace"),
                recursiveOriginal: reader.GetBoolean("Recursive"),
                sortedOriginal: reader.GetBoolean("Sorted"),
                startDirectoryOriginal: reader.GetString("StartDirectory"),
                extraField1Original: reader.GetNullableString("ExtraField1"),
                extraField2Original: reader.GetNullableString("ExtraField2"),
                extraField3Original: reader.GetNullableString("ExtraField3"),
                extraField4Original: reader.GetNullableString("ExtraField4"),
                extraField5Original: reader.GetNullableString("ExtraField5"),
                extraField6Original: reader.GetNullableString("ExtraField6"),
                extraField7Original: reader.GetNullableString("ExtraField7"),
                extraField8Original: reader.GetNullableString("ExtraField8"),
                extraField9Original: reader.GetNullableString("ExtraField9"),
                extraField10Original: reader.GetNullableString("ExtraField10"),
                extraField11Original: reader.GetNullableString("ExtraField11"),
                extraField12Original: reader.GetNullableString("ExtraField12"),
                extraField13Original: reader.GetNullableString("ExtraField13"),
                extraField14Original: reader.GetNullableString("ExtraField14"),
                extraField15Original: reader.GetNullableString("ExtraField15"),
                extraField16Original: reader.GetNullableString("ExtraField16")
            );
        }
    }
}
