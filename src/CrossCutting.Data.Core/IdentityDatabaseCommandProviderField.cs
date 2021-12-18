namespace CrossCutting.Data.Core
{
    public record IdentityDatabaseCommandProviderField
    {
        public string ParameterName { get; }
        public string FieldName { get; }

        public IdentityDatabaseCommandProviderField(string fieldName) : this(fieldName, fieldName)
        {
        }

        public IdentityDatabaseCommandProviderField(string parameterName, string fieldName)
        {
            ParameterName = parameterName;
            FieldName = fieldName;
        }
    }
}
