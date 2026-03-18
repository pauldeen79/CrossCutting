using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityGenerator", @"1.0.0.0")]
    public partial record Catalog
    {
        [ReadOnly(true)]
        public int Id
        {
            get;
        }

        [StringLength(64)]
        [Required]
        public string Name
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public DateTime DateCreated
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public DateTime DateLastModified
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public DateTime DateSynchronized
        {
            get;
        }

        [ReadOnly(true)]
        [StringLength(9)]
        [Required]
        [RegularExpression(@"[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}")]
        public string DriveSerialNumber
        {
            get;
        }

        [DefaultValue(@"CDT")]
        [ReadOnly(true)]
        [StringLength(3)]
        [Required]
        public string DriveTypeCodeType
        {
            get;
        }

        [ReadOnly(true)]
        [StringLength(3)]
        [Required]
        public string DriveTypeCode
        {
            get;
        }

        [ReadOnly(true)]
        public string? DriveTypeDescription
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public int DriveTotalSize
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public int DriveFreeSpace
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public bool Recursive
        {
            get;
        }

        [ReadOnly(true)]
        [Required]
        public bool Sorted
        {
            get;
        }

        [ReadOnly(true)]
        [StringLength(512)]
        [Required]
        public string StartDirectory
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField1
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField2
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField3
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField4
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField5
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField6
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField7
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField8
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField9
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField10
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField11
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField12
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField13
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField14
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField15
        {
            get;
        }

        [StringLength(64)]
        public string? ExtraField16
        {
            get;
        }

        [ReadOnly(true)]
        public bool IsExistingEntity
        {
            get;
        }

        [ReadOnly(true)]
        public int? IdOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? NameOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public DateTime? DateCreatedOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public DateTime? DateLastModifiedOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public DateTime? DateSynchronizedOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? DriveSerialNumberOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? DriveTypeCodeTypeOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? DriveTypeCodeOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public int? DriveTotalSizeOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public int? DriveFreeSpaceOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public bool? RecursiveOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public bool? SortedOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? StartDirectoryOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField1Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField2Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField3Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField4Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField5Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField6Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField7Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField8Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField9Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField10Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField11Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField12Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField13Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField14Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField15Original
        {
            get;
        }

        [ReadOnly(true)]
        public string? ExtraField16Original
        {
            get;
        }

        public Catalog(int id,
                       string name,
                       DateTime dateCreated,
                       DateTime dateLastModified,
                       DateTime dateSynchronized,
                       string driveSerialNumber,
                       string driveTypeCodeType,
                       string driveTypeCode,
                       string? driveTypeDescription,
                       int driveTotalSize,
                       int driveFreeSpace,
                       bool recursive,
                       bool sorted,
                       string startDirectory,
                       string? extraField1,
                       string? extraField2,
                       string? extraField3,
                       string? extraField4,
                       string? extraField5,
                       string? extraField6,
                       string? extraField7,
                       string? extraField8,
                       string? extraField9,
                       string? extraField10,
                       string? extraField11,
                       string? extraField12,
                       string? extraField13,
                       string? extraField14,
                       string? extraField15,
                       string? extraField16,
                       bool isExistingEntity = false,
                       int? idOriginal = default,
                       string? nameOriginal = default,
                       DateTime? dateCreatedOriginal = default,
                       DateTime? dateLastModifiedOriginal = default,
                       DateTime? dateSynchronizedOriginal = default,
                       string? driveSerialNumberOriginal = default,
                       string? driveTypeCodeTypeOriginal = default,
                       string? driveTypeCodeOriginal = default,
                       int? driveTotalSizeOriginal = default,
                       int? driveFreeSpaceOriginal = default,
                       bool? recursiveOriginal = default,
                       bool? sortedOriginal = default,
                       string? startDirectoryOriginal = default,
                       string? extraField1Original = default,
                       string? extraField2Original = default,
                       string? extraField3Original = default,
                       string? extraField4Original = default,
                       string? extraField5Original = default,
                       string? extraField6Original = default,
                       string? extraField7Original = default,
                       string? extraField8Original = default,
                       string? extraField9Original = default,
                       string? extraField10Original = default,
                       string? extraField11Original = default,
                       string? extraField12Original = default,
                       string? extraField13Original = default,
                       string? extraField14Original = default,
                       string? extraField15Original = default,
                       string? extraField16Original = default)
        {
            this.Id = id;
            this.Name = name;
            this.DateCreated = dateCreated;
            this.DateLastModified = dateLastModified;
            this.DateSynchronized = dateSynchronized;
            this.DriveSerialNumber = driveSerialNumber;
            this.DriveTypeCodeType = driveTypeCodeType;
            this.DriveTypeCode = driveTypeCode;
            this.DriveTypeDescription = driveTypeDescription;
            this.DriveTotalSize = driveTotalSize;
            this.DriveFreeSpace = driveFreeSpace;
            this.Recursive = recursive;
            this.Sorted = sorted;
            this.StartDirectory = startDirectory;
            this.ExtraField1 = extraField1;
            this.ExtraField2 = extraField2;
            this.ExtraField3 = extraField3;
            this.ExtraField4 = extraField4;
            this.ExtraField5 = extraField5;
            this.ExtraField6 = extraField6;
            this.ExtraField7 = extraField7;
            this.ExtraField8 = extraField8;
            this.ExtraField9 = extraField9;
            this.ExtraField10 = extraField10;
            this.ExtraField11 = extraField11;
            this.ExtraField12 = extraField12;
            this.ExtraField13 = extraField13;
            this.ExtraField14 = extraField14;
            this.ExtraField15 = extraField15;
            this.ExtraField16 = extraField16;
            this.IsExistingEntity = isExistingEntity;
            this.IdOriginal = idOriginal;
            this.NameOriginal = nameOriginal;
            this.DateCreatedOriginal = dateCreatedOriginal;
            this.DateLastModifiedOriginal = dateLastModifiedOriginal;
            this.DateSynchronizedOriginal = dateSynchronizedOriginal;
            this.DriveSerialNumberOriginal = driveSerialNumberOriginal;
            this.DriveTypeCodeTypeOriginal = driveTypeCodeTypeOriginal;
            this.DriveTypeCodeOriginal = driveTypeCodeOriginal;
            this.DriveTotalSizeOriginal = driveTotalSizeOriginal;
            this.DriveFreeSpaceOriginal = driveFreeSpaceOriginal;
            this.RecursiveOriginal = recursiveOriginal;
            this.SortedOriginal = sortedOriginal;
            this.StartDirectoryOriginal = startDirectoryOriginal;
            this.ExtraField1Original = extraField1Original;
            this.ExtraField2Original = extraField2Original;
            this.ExtraField3Original = extraField3Original;
            this.ExtraField4Original = extraField4Original;
            this.ExtraField5Original = extraField5Original;
            this.ExtraField6Original = extraField6Original;
            this.ExtraField7Original = extraField7Original;
            this.ExtraField8Original = extraField8Original;
            this.ExtraField9Original = extraField9Original;
            this.ExtraField10Original = extraField10Original;
            this.ExtraField11Original = extraField11Original;
            this.ExtraField12Original = extraField12Original;
            this.ExtraField13Original = extraField13Original;
            this.ExtraField14Original = extraField14Original;
            this.ExtraField15Original = extraField15Original;
            this.ExtraField16Original = extraField16Original;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

