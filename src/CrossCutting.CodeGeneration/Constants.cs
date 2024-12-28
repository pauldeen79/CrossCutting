namespace CrossCutting.CodeGeneration;

public static class Constants
{
    public const string ProjectName = "CrossCutting";

    public static class Namespaces
    {
        public const string UtilitiesParsers = "CrossCutting.Utilities.Parsers";
        public const string UtilitiesParsersFunctionCallArguments = "CrossCutting.Utilities.Parsers.FunctionCallArguments";
        public const string UtilitiesParsersBuilders = "CrossCutting.Utilities.Parsers.Builders";
        public const string UtilitiesParsersBuildersFunctionCallArguments = "CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments";
    }

    public static class Types
    {
        public const string FunctionCallArgument = "FunctionCallArgument";

        public const string FunctionCallArgumentBuilder = "FunctionCallArgumentBuilder";
    }

    [ExcludeFromCodeCoverage]
    public static class TypeNames
    {
        public const string FunctionCallArgument = $"{Namespaces.UtilitiesParsers}.FunctionCallArgument";
    }

    [ExcludeFromCodeCoverage]
    public static class Paths
    {
        public const string FunctionCallArguments = $"{Namespaces.UtilitiesParsers}/{nameof(FunctionCallArguments)}";

        public const string FunctionCallArgumentBuilders = $"{Namespaces.UtilitiesParsers}/Builders/{nameof(FunctionCallArguments)}";
    }
}
