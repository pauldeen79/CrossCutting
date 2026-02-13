using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;

namespace PDC.Net.Core.Entities
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Entities.EntityIdentityGenerator", @"1.0.0.0")]
    public partial record ExtraFieldIdentity
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

        public ExtraFieldIdentity(ExtraField instance)
        {
            EntityName = instance.EntityName;
            Name = instance.Name;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }

        public ExtraFieldIdentity(string entityName, string name)
        {
            EntityName = entityName;
            Name = name;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
}

