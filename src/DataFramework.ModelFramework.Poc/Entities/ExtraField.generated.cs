using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityGenerator", @"1.0.0.0")]
    public partial record ExtraField
    {
        [StringLength(64)]
        [Required]
        public string EntityName
        {
            get;
        }

        [StringLength(64)]
        [Required]
        public string Name
        {
            get;
        }

        [StringLength(512)]
        public string? Description
        {
            get;
        }

        public byte FieldNumber
        {
            get;
        }

        [StringLength(64)]
        [Required]
        public string FieldType
        {
            get;
        }

        [ReadOnly(true)]
        public bool IsExistingEntity
        {
            get;
        }

        [ReadOnly(true)]
        public string? EntityNameOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? NameOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? DescriptionOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public byte? FieldNumberOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? FieldTypeOriginal
        {
            get;
        }

        public ExtraField(string entityName,
                          string name,
                          string? description,
                          byte fieldNumber,
                          string fieldType,
                          bool isExistingEntity = false,
                          string? entityNameOriginal = default,
                          string? nameOriginal = default,
                          string? descriptionOriginal = default,
                          byte? fieldNumberOriginal = default,
                          string? fieldTypeOriginal = default)
        {
            this.EntityName = entityName;
            this.Name = name;
            this.Description = description;
            this.FieldNumber = fieldNumber;
            this.FieldType = fieldType;
            this.IsExistingEntity = isExistingEntity;
            this.EntityNameOriginal = entityNameOriginal;
            this.NameOriginal = nameOriginal;
            this.DescriptionOriginal = descriptionOriginal;
            this.FieldNumberOriginal = fieldNumberOriginal;
            this.FieldTypeOriginal = fieldTypeOriginal;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

