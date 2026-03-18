using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityBuilderGenerator", @"1.0.0.0")]
    public partial class ExtraFieldBuilder
    {
        [StringLength(64)]
        [Required]
        public string EntityName
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

        [StringLength(512)]
        public string? Description
        {
            get;
            set;
        }

        public byte FieldNumber
        {
            get;
            set;
        }

        [StringLength(64)]
        [Required]
        public string FieldType
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
        public string? EntityNameOriginal
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
        public string? DescriptionOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public byte? FieldNumberOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? FieldTypeOriginal
        {
            get;
            set;
        }

        public ExtraFieldBuilder WithEntityName(string value)
        {
            this.EntityName = value;
            return this;
        }

        public ExtraFieldBuilder WithName(string value)
        {
            this.Name = value;
            return this;
        }

        public ExtraFieldBuilder WithDescription(string? value)
        {
            this.Description = value;
            return this;
        }

        public ExtraFieldBuilder WithFieldNumber(byte value)
        {
            this.FieldNumber = value;
            return this;
        }

        public ExtraFieldBuilder WithFieldType(string value)
        {
            this.FieldType = value;
            return this;
        }

        public ExtraFieldBuilder WithIsExistingEntity(bool value = true)
        {
            this.IsExistingEntity = value;
            return this;
        }

        public ExtraFieldBuilder WithEntityNameOriginal(string? value)
        {
            this.EntityNameOriginal = value;
            return this;
        }

        public ExtraFieldBuilder WithNameOriginal(string? value)
        {
            this.NameOriginal = value;
            return this;
        }

        public ExtraFieldBuilder WithDescriptionOriginal(string? value)
        {
            this.DescriptionOriginal = value;
            return this;
        }

        public ExtraFieldBuilder WithFieldNumberOriginal(byte? value)
        {
            this.FieldNumberOriginal = value;
            return this;
        }

        public ExtraFieldBuilder WithFieldTypeOriginal(string? value)
        {
            this.FieldTypeOriginal = value;
            return this;
        }

        public ExtraField Build()
        {
            return new ExtraField(EntityName, Name, Description, FieldNumber, FieldType, IsExistingEntity, EntityNameOriginal, NameOriginal, DescriptionOriginal, FieldNumberOriginal, FieldTypeOriginal);
        }

        public ExtraFieldBuilder()
        {
            Name = string.Empty;
            EntityName = string.Empty;
            FieldType = string.Empty;
        }

        public ExtraFieldBuilder(ExtraField instance)
        {
            EntityName = instance.EntityName;
            Name = instance.Name;
            Description = instance.Description;
            FieldNumber = instance.FieldNumber;
            FieldType = instance.FieldType;
            IsExistingEntity = instance.IsExistingEntity;
            EntityNameOriginal = instance.EntityNameOriginal;
            NameOriginal = instance.NameOriginal;
            DescriptionOriginal = instance.DescriptionOriginal;
            FieldNumberOriginal = instance.FieldNumberOriginal;
            FieldTypeOriginal = instance.FieldTypeOriginal;
        }
    }
#nullable restore
}

