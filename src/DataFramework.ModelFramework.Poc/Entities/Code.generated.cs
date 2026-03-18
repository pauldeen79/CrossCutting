using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityGenerator", @"1.0.0.0")]
    public partial record Code
    {
        [DisplayName(@"Code")]
        [StringLength(3)]
        [Required]
        public string CodeProperty
        {
            get;
        }

        [StringLength(3)]
        [Required]
        public string CodeType
        {
            get;
        }

        [StringLength(512)]
        public string? Description
        {
            get;
        }

        [ReadOnly(true)]
        public bool IsExistingEntity
        {
            get;
        }

        [ReadOnly(true)]
        public string? CodeOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? CodeTypeOriginal
        {
            get;
        }

        [ReadOnly(true)]
        public string? DescriptionOriginal
        {
            get;
        }

        public Code(string code,
                    string codeType,
                    string? description,
                    bool isExistingEntity = false,
                    string? codeOriginal = default,
                    string? codeTypeOriginal = default,
                    string? descriptionOriginal = default)
        {
            this.CodeProperty = code;
            this.CodeType = codeType;
            this.Description = description;
            this.IsExistingEntity = isExistingEntity;
            this.CodeOriginal = codeOriginal;
            this.CodeTypeOriginal = codeTypeOriginal;
            this.DescriptionOriginal = descriptionOriginal;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

