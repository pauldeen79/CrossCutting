using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CrossCutting.Data.Core.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public record TestEntityIdentity
    {
        public string Code { get; }
        public string CodeType { get; }

        public TestEntityIdentity(TestEntity instance)
        {
            Code = instance.Code;
            CodeType = instance.CodeType;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }

        public TestEntityIdentity(string code, string codeType)
        {
            Code = code;
            CodeType = codeType;
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
}
