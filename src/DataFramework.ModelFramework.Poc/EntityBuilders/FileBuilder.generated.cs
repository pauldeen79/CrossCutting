using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityBuilderGenerator", @"1.0.0.0")]
    public partial class FileBuilder
    {
        public int Id
        {
            get;
            set;
        }

        [StringLength(512)]
        [Required]
        public string Path
        {
            get;
            set;
        }

        [StringLength(128)]
        [Required]
        public string FileName
        {
            get;
            set;
        }

        [StringLength(12)]
        [Required]
        public string Extension
        {
            get;
            set;
        }

        [Required]
        public System.DateTime Date
        {
            get;
            set;
        }

        [Required]
        public long Size
        {
            get;
            set;
        }

        [Required]
        public int CatalogId
        {
            get;
            set;
        }

        public string? CatalogName
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
        public string? PathOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? FileNameOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? ExtensionOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public System.DateTime? DateOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public long? SizeOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public int? CatalogIdOriginal
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

        public FileBuilder WithId(int value)
        {
            this.Id = value;
            return this;
        }

        public FileBuilder WithPath(string value)
        {
            this.Path = value;
            return this;
        }

        public FileBuilder WithFileName(string value)
        {
            this.FileName = value;
            return this;
        }

        public FileBuilder WithExtension(string value)
        {
            this.Extension = value;
            return this;
        }

        public FileBuilder WithDate(System.DateTime value)
        {
            this.Date = value;
            return this;
        }

        public FileBuilder WithSize(long value)
        {
            this.Size = value;
            return this;
        }

        public FileBuilder WithCatalogId(int value)
        {
            this.CatalogId = value;
            return this;
        }

        public FileBuilder WithCatalogName(string value)
        {
            this.CatalogName = value;
            return this;
        }

        public FileBuilder WithExtraField1(string? value)
        {
            this.ExtraField1 = value;
            return this;
        }

        public FileBuilder WithExtraField2(string? value)
        {
            this.ExtraField2 = value;
            return this;
        }

        public FileBuilder WithExtraField3(string? value)
        {
            this.ExtraField3 = value;
            return this;
        }

        public FileBuilder WithExtraField4(string? value)
        {
            this.ExtraField4 = value;
            return this;
        }

        public FileBuilder WithExtraField5(string? value)
        {
            this.ExtraField5 = value;
            return this;
        }

        public FileBuilder WithExtraField6(string? value)
        {
            this.ExtraField6 = value;
            return this;
        }

        public FileBuilder WithExtraField7(string? value)
        {
            this.ExtraField7 = value;
            return this;
        }

        public FileBuilder WithExtraField8(string? value)
        {
            this.ExtraField8 = value;
            return this;
        }

        public FileBuilder WithExtraField9(string? value)
        {
            this.ExtraField9 = value;
            return this;
        }

        public FileBuilder WithExtraField10(string? value)
        {
            this.ExtraField10 = value;
            return this;
        }

        public FileBuilder WithExtraField11(string? value)
        {
            this.ExtraField11 = value;
            return this;
        }

        public FileBuilder WithExtraField12(string? value)
        {
            this.ExtraField12 = value;
            return this;
        }

        public FileBuilder WithExtraField13(string? value)
        {
            this.ExtraField13 = value;
            return this;
        }

        public FileBuilder WithExtraField14(string? value)
        {
            this.ExtraField14 = value;
            return this;
        }

        public FileBuilder WithExtraField15(string? value)
        {
            this.ExtraField15 = value;
            return this;
        }

        public FileBuilder WithExtraField16(string? value)
        {
            this.ExtraField16 = value;
            return this;
        }

        public FileBuilder WithIsExistingEntity(bool value = true)
        {
            this.IsExistingEntity = value;
            return this;
        }

        public FileBuilder WithIdOriginal(int? value)
        {
            this.IdOriginal = value;
            return this;
        }

        public FileBuilder WithPathOriginal(string? value)
        {
            this.PathOriginal = value;
            return this;
        }

        public FileBuilder WithFileNameOriginal(string? value)
        {
            this.FileNameOriginal = value;
            return this;
        }

        public FileBuilder WIthExtensionOriginal(string? value)
        {
            this.ExtensionOriginal = value;
            return this;
        }

        public FileBuilder WithDateOriginal(System.DateTime? value)
        {
            this.DateOriginal = value;
            return this;
        }

        public FileBuilder WithSizeOriginal(long? value)
        {
            this.SizeOriginal = value;
            return this;
        }

        public FileBuilder WithCatalogIdOriginal(int? value)
        {
            this.CatalogIdOriginal = value;
            return this;
        }

        public FileBuilder WithExtraField1Original(string? value)
        {
            this.ExtraField1Original = value;
            return this;
        }

        public FileBuilder WithExtraField2Original(string? value)
        {
            this.ExtraField2Original = value;
            return this;
        }

        public FileBuilder WithExtraField3Original(string? value)
        {
            this.ExtraField3Original = value;
            return this;
        }

        public FileBuilder WithExtraField4Original(string? value)
        {
            this.ExtraField4Original = value;
            return this;
        }

        public FileBuilder WithExtraField5Original(string? value)
        {
            this.ExtraField5Original = value;
            return this;
        }

        public FileBuilder WithExtraField6Original(string? value)
        {
            this.ExtraField6Original = value;
            return this;
        }

        public FileBuilder WithExtraField7Original(string? value)
        {
            this.ExtraField7Original = value;
            return this;
        }

        public FileBuilder WithExtraField8Original(string? value)
        {
            this.ExtraField8Original = value;
            return this;
        }

        public FileBuilder WithExtraField9Original(string? value)
        {
            this.ExtraField9Original = value;
            return this;
        }

        public FileBuilder WithExtraField10Original(string? value)
        {
            this.ExtraField10Original = value;
            return this;
        }

        public FileBuilder WithExtraField11Original(string? value)
        {
            this.ExtraField11Original = value;
            return this;
        }

        public FileBuilder WithExtraField12Original(string? value)
        {
            this.ExtraField12Original = value;
            return this;
        }

        public FileBuilder WithExtraField13Original(string? value)
        {
            this.ExtraField13Original = value;
            return this;
        }

        public FileBuilder WithExtraField14Original(string? value)
        {
            this.ExtraField14Original = value;
            return this;
        }

        public FileBuilder WithExtraField15Original(string? value)
        {
            this.ExtraField15Original = value;
            return this;
        }

        public FileBuilder WithExtraField16Original(string? value)
        {
            this.ExtraField16Original = value;
            return this;
        }

        public File Build()
        {
            return new File(Id, Path, FileName, Extension, Date, Size, CatalogId, CatalogName, ExtraField1, ExtraField2, ExtraField3, ExtraField4, ExtraField5, ExtraField6, ExtraField7, ExtraField8, ExtraField9, ExtraField10, ExtraField11, ExtraField12, ExtraField13, ExtraField14, ExtraField15, ExtraField16, IsExistingEntity, IdOriginal, PathOriginal, FileNameOriginal, ExtensionOriginal, DateOriginal, SizeOriginal, CatalogIdOriginal, ExtraField1Original, ExtraField2Original, ExtraField3Original, ExtraField4Original, ExtraField5Original, ExtraField6Original, ExtraField7Original, ExtraField8Original, ExtraField9Original, ExtraField10Original, ExtraField11Original, ExtraField12Original, ExtraField13Original, ExtraField14Original, ExtraField15Original, ExtraField16Original);
        }

        public FileBuilder()
        {
            FileName = string.Empty;
            Path = string.Empty;
            Extension = string.Empty;
        }

        public FileBuilder(File instance)
        {
            Id = instance.Id;
            Path = instance.Path;
            FileName = instance.FileName;
            Extension = instance.Extension;
            Date = instance.Date;
            Size = instance.Size;
            CatalogId = instance.CatalogId;
            CatalogName = instance.CatalogName;
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
            PathOriginal = instance.PathOriginal;
            FileNameOriginal = instance.FileNameOriginal;
            ExtensionOriginal = instance.ExtensionOriginal;
            DateOriginal = instance.DateOriginal;
            SizeOriginal = instance.SizeOriginal;
            CatalogIdOriginal = instance.CatalogIdOriginal;
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

