using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityBuilderGenerator", @"1.0.0.0")]
    public partial class CodeBuilder
    {
        [StringLength(3)]
        [Required]
        public string Code
        {
            get;
            set;
        }

        [StringLength(3)]
        [Required]
        public string CodeType
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

        public bool IsExistingEntity
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? CodeOriginal
        {
            get;
            set;
        }

        [ReadOnly(true)]
        public string? CodeTypeOriginal
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

        public CodeBuilder WithCode(string value)
        {
            this.Code = value;
            return this;
        }

        public CodeBuilder WithCodeType(string value)
        {
            this.CodeType = value;
            return this;
        }

        public CodeBuilder WithDescription(string value)
        {
            this.Description = value;
            return this;
        }

        public CodeBuilder WithIsExistingEntity(bool value = true)
        {
            this.IsExistingEntity = value;
            return this;
        }

        public CodeBuilder WithCodeOriginal(string? value)
        {
            this.CodeOriginal = value;
            return this;
        }

        public CodeBuilder WithCodeTypeOriginal(string? value)
        {
            this.CodeTypeOriginal = value;
            return this;
        }

        public CodeBuilder WithDescriptionOriginal(string? value)
        {
            this.DescriptionOriginal = value;
            return this;
        }

        public Code Build()
        {
            return new Code(Code, CodeType, Description, IsExistingEntity, CodeOriginal, CodeTypeOriginal, DescriptionOriginal);
        }

        public CodeBuilder()
        {
            Code = string.Empty;
            CodeType = string.Empty;
        }

        public CodeBuilder(Code instance)
        {
            Code = instance.CodeProperty;
            CodeType = instance.CodeType;
            Description = instance.Description;
            IsExistingEntity = instance.IsExistingEntity;
            CodeOriginal = instance.CodeOriginal;
            CodeTypeOriginal = instance.CodeTypeOriginal;
            DescriptionOriginal = instance.DescriptionOriginal;
        }
    }
#nullable restore
}

