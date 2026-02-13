using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityBuilderGenerator", @"1.0.0.0")]
    public partial class CatalogBuilder
    {
        public int Id
        {
            get;
            set;
        }

        [StringLength(64)]
        [Required]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public DateTime DateCreated
        {
            get;
            set;
        }

        [Required]
        public DateTime DateLastModified
        {
            get;
            set;
        }

        [Required]
        public DateTime DateSynchronized
        {
            get;
            set;
        }

        [StringLength(9)]
        [Required]
        [RegularExpression(@"[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}")]
        public string DriveSerialNumber
        {
            get;
            set;
        }

        [DefaultValue(@"CDT")]
        [StringLength(3)]
        [Required]
        public string DriveTypeCodeType
        {
            get;
            set;
        }

        [StringLength(3)]
        [Required]
        public string DriveTypeCode
        {
            get;
            set;
        }

        public string? DriveTypeDescription
        {
            get;
            set;
        }

        [Required]
        public int DriveTotalSize
        {
            get;
            set;
        }

        [Required]
        public int DriveFreeSpace
        {
            get;
            set;
        }

        [Required]
        public bool Recursive
        {
            get;
            set;
        }

        [Required]
        public bool Sorted
        {
            get;
            set;
        }

        [StringLength(512)]
        [Required]
        public string StartDirectory
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField1
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField2
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField3
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField4
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField5
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField6
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField7
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField8
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField9
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField10
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField11
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField12
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField13
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField14
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField15
        {
            get;
            set;
        }

        [StringLength(64)]
        public string? ExtraField16
        {
            get;
            set;
        }

        public bool IsExistingEntity
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public int? IdOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? NameOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public DateTime? DateCreatedOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public DateTime? DateLastModifiedOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public DateTime? DateSynchronizedOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? DriveSerialNumberOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? DriveTypeCodeTypeOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? DriveTypeCodeOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public int? DriveTotalSizeOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public int? DriveFreeSpaceOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public bool? RecursiveOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public bool? SortedOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? StartDirectoryOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField1Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField2Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField3Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField4Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField5Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField6Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField7Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField8Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField9Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField10Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField11Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField12Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField13Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField14Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField15Original
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtraField16Original
        {
            get;
            set;
        }

        public CatalogBuilder WithId(int value)
        {
            this.Id = value;
            return this;
        }

        public CatalogBuilder WithName(string value)
        {
            this.Name = value;
            return this;
        }

        public CatalogBuilder WithDateCreated(DateTime value)
        {
            this.DateCreated = value;
            return this;
        }

        public CatalogBuilder WithDateLastModified(DateTime value)
        {
            this.DateLastModified = value;
            return this;
        }

        public CatalogBuilder WithDateSynchronized(DateTime value)
        {
            this.DateSynchronized = value;
            return this;
        }

        public CatalogBuilder WithDriveSerialNumber(string value)
        {
            this.DriveSerialNumber = value;
            return this;
        }

        public CatalogBuilder WithDriveTypeCodeType(string value)
        {
            this.DriveTypeCodeType = value;
            return this;
        }

        public CatalogBuilder WithDriveTypeCode(string value)
        {
            this.DriveTypeCode = value;
            return this;
        }

        public CatalogBuilder WithDriveTypeDescription(string? value)
        {
            this.DriveTypeDescription = value;
            return this;
        }

        public CatalogBuilder WithDriveTotalSize(int value)
        {
            this.DriveTotalSize = value;
            return this;
        }

        public CatalogBuilder WithDriveFreeSpace(int value)
        {
            this.DriveFreeSpace = value;
            return this;
        }

        public CatalogBuilder WithRecursive(bool value = true)
        {
            this.Recursive = value;
            return this;
        }

        public CatalogBuilder WithSorted(bool value = true)
        {
            this.Sorted = value;
            return this;
        }

        public CatalogBuilder WithStartDirectory(string value)
        {
            this.StartDirectory = value;
            return this;
        }

        public CatalogBuilder WithExtraField1(string? value)
        {
            this.ExtraField1 = value;
            return this;
        }

        public CatalogBuilder WithExtraField2(string? value)
        {
            this.ExtraField2 = value;
            return this;
        }

        public CatalogBuilder WithExtraField3(string? value)
        {
            this.ExtraField3 = value;
            return this;
        }

        public CatalogBuilder WithExtraField4(string? value)
        {
            this.ExtraField4 = value;
            return this;
        }

        public CatalogBuilder WithExtraField5(string? value)
        {
            this.ExtraField5 = value;
            return this;
        }

        public CatalogBuilder WithExtraField6(string? value)
        {
            this.ExtraField6 = value;
            return this;
        }

        public CatalogBuilder WithExtraField7(string? value)
        {
            this.ExtraField7 = value;
            return this;
        }

        public CatalogBuilder WithExtraField8(string? value)
        {
            this.ExtraField8 = value;
            return this;
        }

        public CatalogBuilder WithExtraField9(string? value)
        {
            this.ExtraField9 = value;
            return this;
        }

        public CatalogBuilder WithExtraField10(string? value)
        {
            this.ExtraField10 = value;
            return this;
        }

        public CatalogBuilder WithExtraField11(string? value)
        {
            this.ExtraField11 = value;
            return this;
        }

        public CatalogBuilder WithExtraField12(string? value)
        {
            this.ExtraField12 = value;
            return this;
        }

        public CatalogBuilder WithExtraField13(string? value)
        {
            this.ExtraField13 = value;
            return this;
        }

        public CatalogBuilder WithExtraField14(string? value)
        {
            this.ExtraField14 = value;
            return this;
        }

        public CatalogBuilder WithExtraField15(string? value)
        {
            this.ExtraField15 = value;
            return this;
        }

        public CatalogBuilder WithExtraField16(string? value)
        {
            this.ExtraField16 = value;
            return this;
        }

        public CatalogBuilder WithIsExistingEntity(bool value)
        {
            this.IsExistingEntity = value;
            return this;
        }

        public CatalogBuilder WithIdOriginal(int? value)
        {
            this.IdOriginal = value;
            return this;
        }

        public CatalogBuilder WithNameOriginal(string? value)
        {
            this.NameOriginal = value;
            return this;
        }

        public CatalogBuilder WithDateCreatedOriginal(DateTime? value)
        {
            this.DateCreatedOriginal = value;
            return this;
        }

        public CatalogBuilder WithDateLastModifiedOriginal(DateTime? value)
        {
            this.DateLastModifiedOriginal = value;
            return this;
        }

        public CatalogBuilder WithDateSynchronizedOriginal(DateTime? value)
        {
            this.DateSynchronizedOriginal = value;
            return this;
        }

        public CatalogBuilder WithDriveSerialNumberOriginal(string? value)
        {
            this.DriveSerialNumberOriginal = value;
            return this;
        }

        public CatalogBuilder WithDriveTypeCodeTypeOriginal(string? value)
        {
            this.DriveTypeCodeTypeOriginal = value;
            return this;
        }

        public CatalogBuilder WithDriveTypeCodeOriginal(string? value)
        {
            this.DriveTypeCodeOriginal = value;
            return this;
        }

        public CatalogBuilder WithDriveTotalSizeOriginal(int? value)
        {
            this.DriveTotalSizeOriginal = value;
            return this;
        }

        public CatalogBuilder WithDriveFreeSpaceOriginal(int? value)
        {
            this.DriveFreeSpaceOriginal = value;
            return this;
        }

        public CatalogBuilder WithRecursiveOriginal(bool? value = true)
        {
            this.RecursiveOriginal = value;
            return this;
        }

        public CatalogBuilder WithSortedOriginal(bool? value = true)
        {
            this.SortedOriginal = value;
            return this;
        }

        public CatalogBuilder WithStartDirectoryOriginal(string? value)
        {
            this.StartDirectoryOriginal = value;
            return this;
        }

        public CatalogBuilder WithExtraField1Original(string? value)
        {
            this.ExtraField1Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField2Original(string? value)
        {
            this.ExtraField2Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField3Original(string? value)
        {
            this.ExtraField3Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField4Original(string? value)
        {
            this.ExtraField4Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField5Original(string? value)
        {
            this.ExtraField5Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField6Original(string? value)
        {
            this.ExtraField6Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField7Original(string? value)
        {
            this.ExtraField7Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField8Original(string? value)
        {
            this.ExtraField8Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField9Original(string? value)
        {
            this.ExtraField9Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField10Original(string? value)
        {
            this.ExtraField10Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField11Original(string? value)
        {
            this.ExtraField11Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField12Original(string? value)
        {
            this.ExtraField12Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField13Original(string? value)
        {
            this.ExtraField13Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField14Original(string? value)
        {
            this.ExtraField14Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField15Original(string? value)
        {
            this.ExtraField15Original = value;
            return this;
        }

        public CatalogBuilder WithExtraField16Original(string? value)
        {
            this.ExtraField16Original = value;
            return this;
        }

        public Catalog Build()
        {
            return new Catalog(Id, Name, DateCreated, DateLastModified, DateSynchronized, DriveSerialNumber, DriveTypeCodeType, DriveTypeCode, DriveTypeDescription, DriveTotalSize, DriveFreeSpace, Recursive, Sorted, StartDirectory, ExtraField1, ExtraField2, ExtraField3, ExtraField4, ExtraField5, ExtraField6, ExtraField7, ExtraField8, ExtraField9, ExtraField10, ExtraField11, ExtraField12, ExtraField13, ExtraField14, ExtraField15, ExtraField16, IsExistingEntity, IdOriginal, NameOriginal, DateCreatedOriginal, DateLastModifiedOriginal, DateSynchronizedOriginal, DriveSerialNumberOriginal, DriveTypeCodeTypeOriginal, DriveTypeCodeOriginal, DriveTotalSizeOriginal, DriveFreeSpaceOriginal, RecursiveOriginal, SortedOriginal, StartDirectoryOriginal, ExtraField1Original, ExtraField2Original, ExtraField3Original, ExtraField4Original, ExtraField5Original, ExtraField6Original, ExtraField7Original, ExtraField8Original, ExtraField9Original, ExtraField10Original, ExtraField11Original, ExtraField12Original, ExtraField13Original, ExtraField14Original, ExtraField15Original, ExtraField16Original);
        }

        public CatalogBuilder()
        {
            Name = string.Empty;
            DriveSerialNumber = "0000-0000";
            DriveTypeCodeType = "CDT";
            DriveTypeCode = string.Empty;
            DriveTypeDescription = string.Empty;
            StartDirectory = string.Empty;
        }

        public CatalogBuilder(Catalog instance)
        {
            Id = instance.Id;
            Name = instance.Name;
            DateCreated = instance.DateCreated;
            DateLastModified = instance.DateLastModified;
            DateSynchronized = instance.DateSynchronized;
            DriveSerialNumber = instance.DriveSerialNumber;
            DriveTypeCodeType = instance.DriveTypeCodeType;
            DriveTypeCode = instance.DriveTypeCode;
            DriveTypeDescription = instance.DriveTypeDescription;
            DriveTotalSize = instance.DriveTotalSize;
            DriveFreeSpace = instance.DriveFreeSpace;
            Recursive = instance.Recursive;
            Sorted = instance.Sorted;
            StartDirectory = instance.StartDirectory;
            ExtraField1 = instance.ExtraField1;
            ExtraField2 = instance.ExtraField2;
            ExtraField3 = instance.ExtraField3;
            ExtraField4 = instance.ExtraField4;
            ExtraField5 = instance.ExtraField5;
            ExtraField6 = instance.ExtraField6;
            ExtraField7 = instance.ExtraField7;
            ExtraField8 = instance.ExtraField8;
            ExtraField9 = instance.ExtraField9;
            ExtraField10 = instance.ExtraField10;
            ExtraField11 = instance.ExtraField11;
            ExtraField12 = instance.ExtraField12;
            ExtraField13 = instance.ExtraField13;
            ExtraField14 = instance.ExtraField14;
            ExtraField15 = instance.ExtraField15;
            ExtraField16 = instance.ExtraField16;
            IsExistingEntity = instance.IsExistingEntity;
            IdOriginal = instance.IdOriginal;
            NameOriginal = instance.NameOriginal;
            DateCreatedOriginal = instance.DateCreatedOriginal;
            DateLastModifiedOriginal = instance.DateLastModifiedOriginal;
            DateSynchronizedOriginal = instance.DateSynchronizedOriginal;
            DriveSerialNumberOriginal = instance.DriveSerialNumberOriginal;
            DriveTypeCodeTypeOriginal = instance.DriveTypeCodeTypeOriginal;
            DriveTypeCodeOriginal = instance.DriveTypeCodeOriginal;
            DriveTotalSizeOriginal = instance.DriveTotalSizeOriginal;
            DriveFreeSpaceOriginal = instance.DriveFreeSpaceOriginal;
            RecursiveOriginal = instance.RecursiveOriginal;
            SortedOriginal = instance.SortedOriginal;
            StartDirectoryOriginal = instance.StartDirectoryOriginal;
            ExtraField1Original = instance.ExtraField1Original;
            ExtraField2Original = instance.ExtraField2Original;
            ExtraField3Original = instance.ExtraField3Original;
            ExtraField4Original = instance.ExtraField4Original;
            ExtraField5Original = instance.ExtraField5Original;
            ExtraField6Original = instance.ExtraField6Original;
            ExtraField7Original = instance.ExtraField7Original;
            ExtraField8Original = instance.ExtraField8Original;
            ExtraField9Original = instance.ExtraField9Original;
            ExtraField10Original = instance.ExtraField10Original;
            ExtraField11Original = instance.ExtraField11Original;
            ExtraField12Original = instance.ExtraField12Original;
            ExtraField13Original = instance.ExtraField13Original;
            ExtraField14Original = instance.ExtraField14Original;
            ExtraField15Original = instance.ExtraField15Original;
            ExtraField16Original = instance.ExtraField16Original;
        }
    }
#nullable restore
}

